using Application.Common;
using Application.Helpers;
using Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

namespace Application.Users.Commands;

public record class LoginCommand : IRequest<BaseResponse<AccessTokenResponseModel>>
{
    [Required]
    public string Username { get; init; }
    [Required]
    public string Password { get; init; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResponse<AccessTokenResponseModel>>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<BaseResponse<AccessTokenResponseModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var applicationName = _configuration.GetValue<string>("ApplicationName") ?? "MyApp";
        var user = new ApplicationUser();
        // find user in database
        if (IsEmail(request.Username))
        {
            user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Username);
        }
        else
        {
            user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
        }

        // user is not existed
        if (user == null)
        {
            return new BaseResponse<AccessTokenResponseModel> { Success = false, Message = "User is not existed" };
        }

        // user is not confirm mail yet
        if(IsEmail(request.Username) && !await _userManager.IsEmailConfirmedAsync(user))
        {
            return new BaseResponse<AccessTokenResponseModel> { Success = false, Message = "User is not confirm mail yet" };
        }

        // perform sign-in
        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, lockoutOnFailure: true);

        // sign-in failed
        if (!result.Succeeded)
        {
            return new BaseResponse<AccessTokenResponseModel> { Success = false, Message = "Username or Password is not correct" };
        }

        // generate access token
        var accessToken = JwtHelper.generateAccessToken(user, _configuration,_userManager);


        // remove old refresh token
        await _userManager.RemoveAuthenticationTokenAsync(user, applicationName, "refreshToken");

        // generate new token
        //var refreshToken = await _userManager.GenerateUserTokenAsync(user, applicationName, "refreshToken");
        var refreshToken = JwtHelper.generateRefreshToken(user, _configuration, _userManager);

        // set token 
        await _userManager.SetAuthenticationTokenAsync(user, applicationName, "refreshToken", refreshToken);

        // jwt expired time
        var jwtExpiresIn = _configuration.GetValue<int>("Jwt:ExpiresIn");

        return new BaseResponse<AccessTokenResponseModel>
        {
            Success = true,
            Message = "Sign in successfully",
            Data = new AccessTokenResponseModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = jwtExpiresIn
            }
        };
    }
    bool IsEmail(string email)
    {
        // Simple regex pattern for validating email addresses
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}