using BeanMind.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace BeanMind.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);

    public Task<IdentityResult> SignUpAsync(SignUpModel model);
    public Task<string> SignInAsync(SignInModel model);
}
