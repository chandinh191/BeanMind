using Application.Common;
using Application.Helpers;
using Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Users.Commands;

public record class LoginGoogleCommand : IRequest<BaseResponse<AccessTokenResponseModel>>
{
    public required string IdToken { get; init; }
}

public class LoginGoogleCommandHandler : IRequestHandler<LoginGoogleCommand, BaseResponse<AccessTokenResponseModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public LoginGoogleCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
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
        if(EmailFromToken.IsNullOrEmpty())
        {
            return new BaseResponse<AccessTokenResponseModel> { Success = false, Message = "IdToken missing email value" };
        }

        // find user in db with email
        var user = await _userManager.FindByEmailAsync(EmailFromToken);

        // user founded, generate accessToken, refreshToken return to user
        if(user is not null)
        {
            // user is not confirm mail yet
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new BaseResponse<AccessTokenResponseModel> { Success = false, Message = "User is not confirm mail yet" };
            }

            return await GenerateTokenAndRespond(user, applicationName, jwtExpiresIn, "Login successfully");
        }

        // user not founded, create new user with UserName, Email, generate accessToken, refreshToken return to user
        var newUser = new ApplicationUser { Email = EmailFromToken, UserName = EmailFromToken };
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
        await _userManager.AddToRoleAsync(newUser, Domain.Constants.Roles.Administrator);

        // mail confirmed
        var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _userManager.ConfirmEmailAsync(user, confirmToken);

        return await GenerateTokenAndRespond(newUser, applicationName, jwtExpiresIn, "Account registered and Login successfully");
    }

    private async Task<BaseResponse<AccessTokenResponseModel>> GenerateTokenAndRespond(ApplicationUser user, string applicationName, int jwtExpiresIn, string successMessage)
    {
        var accessTokenForUser = JwtHelper.generateAccessToken(user, _configuration, _userManager);
        await _userManager.RemoveAuthenticationTokenAsync(user, applicationName, "refreshToken");
        //var refreshTokenForUser = await _userManager.GenerateUserTokenAsync(user, applicationName, "refreshToken");
        var refreshTokenForUser = JwtHelper.generateRefreshToken(user, _configuration, _userManager);
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
