using AutoMapper;
using Domain.Entities.UserEntities;

namespace Application.Users;

public sealed record class AccessTokenResponseModel
{
    public string TokenType { get; } = "Bearer";
    public required string AccessToken { get; init; }
    public required long ExpiresIn { get; init; }
    public required string RefreshToken { get; init; }
}

[AutoMap(typeof(ApplicationUser))]
public sealed record class GetUserInfoResponseModel
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required bool EmailConfirmed { get; set; }


    // custom attrib
    public required List<string> Roles { get; set; }
}
