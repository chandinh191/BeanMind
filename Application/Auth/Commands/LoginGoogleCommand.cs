using Application.Common;
using Application.Helpers;
using Application.Orders.Commands;
using Application.Users;
using Application.Users.Commands;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Auth.Commands;

public record class LoginGoogleCommand : IRequest<BaseResponse<AccessTokenResponseModel>>
{
    public required string IdToken { get; init; }
}

public class LoginGoogleCommandHandler : IRequestHandler<LoginGoogleCommand, BaseResponse<AccessTokenResponseModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ISender _sender;

    public LoginGoogleCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, ISender sender)
    {
        _userManager = userManager;
        _configuration = configuration;
        _sender = sender;
    }

    public async Task<BaseResponse<AccessTokenResponseModel>> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
    {
        // jwt expired time
        var jwtExpiresIn = _configuration.GetValue<int>("Jwt:ExpiresIn");

        // application name
        var applicationName = _configuration.GetValue<string>("ApplicationName") ?? "MyApp";

        var tokenHandler = new JwtSecurityTokenHandler();

        // read token claim
        var claims = tokenHandler.ReadJwtToken(request.IdToken).Claims;

        // extract data from IdToken to get Email
        var EmailFromToken = claims.FirstOrDefault(x => x.Type == "email")?.Value;
        //var UserNameFromToken = claims.FirstOrDefault(x => x.Type == "name")?.Value;

        // token data not valid
        if (EmailFromToken.IsNullOrEmpty())
        {
            return new BaseResponse<AccessTokenResponseModel> { Success = false, Message = "IdToken missing email value" };
        }

        // find user in db with email
        var user = await _userManager.FindByEmailAsync(EmailFromToken);

        // user founded, generate accessToken, refreshToken return to user
        if (user is not null)
        {
            return await GenerateTokenAndRespond(user, applicationName, jwtExpiresIn, "Login successfully");
        }

        /*  // user not founded, create new user with UserName, Email, generate accessToken, refreshToken return to user
          var newUser = new ApplicationUser { Email = EmailFromToken, UserName = EmailFromToken ,EmailConfirmed =true};
          var createUserResult = await _userManager.CreateAsync(newUser, RandomGenerator.GenerateRandomString(20));
          // create new user fail
          if (!createUserResult.Succeeded)
          {
              return new BaseResponse<AccessTokenResponseModel>()
              {
                  Success = false,
                  Message = "Create user failed",
                  Errors = createUserResult.Errors.Select(e => e.Description).ToList()
              };
          }
          // add user to role
          await _userManager.AddToRoleAsync(newUser, Domain.Constants.Roles.Parent);
          // mail confirmed
          var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
          await _userManager.ConfirmEmailAsync(user, confirmToken);*/
        var name = claims.FirstOrDefault(x => x.Type == "name")?.Value;
        var firstname2= "";
        var lastname2 = "";
        if (!string.IsNullOrEmpty(name))
        {
            var nameParts = name.Split(' ');
            firstname2 = nameParts.First(); // Lấy chữ đầu tiên làm FirstName
            lastname2 = string.Join(" ", nameParts.Skip(1)); // Phần còn lại làm LastName
        }
        else
        {
            firstname2 = string.Empty;
            lastname2 = string.Empty;
        }

        var imageURl = claims.FirstOrDefault(x => x.Type == "picture")?.Value;
        List<string> roles = new List<string> { "Parent" };
        var result = await _sender.Send(new RegisterUserCommand
        {
            Email = EmailFromToken,
            FirstName = firstname2,
            LastName = lastname2,
            Username = EmailFromToken,
            Password = "Abc@123!",
            EmailConfirmed = true,
            Roles = roles
        });
        
        var newUser = new ApplicationUser {Id=result.Data.Id, Email = EmailFromToken, UserName = EmailFromToken, EmailConfirmed = true };
        //await _userManager.AddToRoleAsync(newUser, Domain.Constants.Roles.Parent);
        return await GenerateTokenAndRespond(newUser, applicationName, jwtExpiresIn, "Account registered and Login successfully");
    }

    private async Task<BaseResponse<AccessTokenResponseModel>> GenerateTokenAndRespond(ApplicationUser user, string applicationName, int jwtExpiresIn, string successMessage)
    {
        var accessTokenForUser = JwtHelper.generateAccessToken(user, _configuration,_userManager);
        await _userManager.RemoveAuthenticationTokenAsync(user, applicationName, "refreshToken");
        //var refreshTokenForUser = await _userManager.GenerateUserTokenAsync(user, applicationName, "refreshToken");
        var refreshTokenForUser = JwtHelper.generateRefreshToken(user, _configuration,_userManager);
        await _userManager.SetAuthenticationTokenAsync(user, applicationName, "refreshToken", refreshTokenForUser);

        return new BaseResponse<AccessTokenResponseModel>
        {
            Success = true,
            Message = successMessage,
            Data = new AccessTokenResponseModel
            {
                AccessToken = accessTokenForUser,
                RefreshToken = refreshTokenForUser,
                ExpiresIn = jwtExpiresIn
            }
        };
    }
}
