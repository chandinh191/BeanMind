using Application.Common;
using Application.Helpers;
using Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Users.Commands;

public record class TokenRefreshCommand : IRequest<BaseResponse<AccessTokenResponseModel>>
{
    //public required string Email { get; set; } // I need to find a way to use ClaimPrinciple so this is temporary solution
    public required string RefreshToken { get; init; }
}

public class TokenRefreshCommandHandler : IRequestHandler<TokenRefreshCommand, BaseResponse<AccessTokenResponseModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public TokenRefreshCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<BaseResponse<AccessTokenResponseModel>> Handle(TokenRefreshCommand request, CancellationToken cancellationToken)
    {
        var applicationName = _configuration.GetValue<string>("ApplicationName") ?? "MyApp";

        // get email from token
        var tokenHandler = new JwtSecurityTokenHandler();
        var emailToken = tokenHandler.ReadJwtToken(request.RefreshToken).Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        if (emailToken is null)
        {
            return new BaseResponse<AccessTokenResponseModel> { Success = false, Message = "Invalid token value" };
        }

        // get user
        var user = await _userManager.FindByEmailAsync(emailToken);

        // user is not existed
        if(user is null)
        {
            return new BaseResponse<AccessTokenResponseModel>
            {
                Success = false,
                Message = "User is not existed",
            };
        }

        // validate refresh token
        string refreshTokenDb = await _userManager.GetAuthenticationTokenAsync(user, applicationName, "refreshToken");

        if (request.RefreshToken != refreshTokenDb)
        {
            return new BaseResponse<AccessTokenResponseModel>
            {
                Success = false,
                Message = "Refresh Token Invalid",
                Errors = ["Refresh Token Invalid"]
            };
        }

        // remove old refresh token
        await _userManager.RemoveAuthenticationTokenAsync(user, applicationName, "refreshToken");

        // generate access token
        var nextAccessToken = JwtHelper.generateAccessToken(user, _configuration, _userManager);

        // create new refresh token
        //var nextRefreshToken = await _userManager.GenerateUserTokenAsync(user, applicationName, "refreshToken");
        var nextRefreshToken = JwtHelper.generateRefreshToken(user, _configuration, _userManager);

        // set token 
        await _userManager.SetAuthenticationTokenAsync(user, applicationName, "refreshToken", nextRefreshToken);

        // jwt expired time
        var jwtExpiresIn = _configuration.GetValue<int>("Jwt:ExpiresIn");

        return new BaseResponse<AccessTokenResponseModel>
        {
            Success = true,
            Message = "Exchange Refresh Token Successfully",
            Data = new AccessTokenResponseModel
            {
                AccessToken = nextAccessToken,
                RefreshToken = nextRefreshToken,
                ExpiresIn = jwtExpiresIn,
            }
        };
    }
}
