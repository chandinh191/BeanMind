using Application.Common;
using Application.Helpers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Application.Users.Commands;

public record class LoginCommand : IRequest<BaseResponse<AccessTokenResponseModel>>
{
    [Required]
    public string Email { get; init; }
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

        // find user in database
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        // user is not existed
        if (user == null)
        {
            return new BaseResponse<AccessTokenResponseModel> { Success = false, Message = "User is not existed" };
        }

        // user is not confirm mail yet
        if(!await _userManager.IsEmailConfirmedAsync(user))
        {
            return new BaseResponse<AccessTokenResponseModel> { Success = false, Message = "User is not confirm mail yet" };
        }

        // perform sign-in
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: true);

        // sign-in failed
        if (!result.Succeeded)
        {
            return new BaseResponse<AccessTokenResponseModel> { Success = false, Message = "Username or Password is not correct" };
        }

        // generate access token
        var accessToken = JwtHelper.generateAccessToken(user, _configuration);


        // remove old refresh token
        await _userManager.RemoveAuthenticationTokenAsync(user, applicationName, "refreshToken");

        // generate new token
        //var refreshToken = await _userManager.GenerateUserTokenAsync(user, applicationName, "refreshToken");
        var refreshToken = JwtHelper.generateRefreshToken(user, _configuration);

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
}