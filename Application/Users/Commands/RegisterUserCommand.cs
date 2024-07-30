using MediatR;
using Application.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Routing;
using Infrastructure.Services;
using System.Text.RegularExpressions;
using Domain.Constants;
using Domain.Entities.UserEntities;

namespace Application.Users.Commands;

public record class RegisterUserCommand : IRequest<BaseResponse<string>>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required List<string> Roles { get; init; }

}
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, BaseResponse<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;
    private readonly LinkGenerator _linkGenerator;
    private readonly IEmailService _emailService;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, LinkGenerator linkGenerator, IEmailService emailService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _linkGenerator = linkGenerator;
        _emailService = emailService;
    }

    public async Task<BaseResponse<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // checking role name invalid
            foreach (var role in request.Roles)
            {
                if (!Roles.AllRoles.Contains(role.ToString()))
                {
                    return new BaseResponse<string>()
                    {
                        Success = false,
                        Message = "Invalid role: " + role,
                        Errors = ["Invalid role: " + role]
                    };
                }
            }

            // find user with username in database
            var existedUser = await _userManager.FindByEmailAsync(request.Username);

            // user with username already existed
            if (existedUser is not null)
            {
                return new BaseResponse<string>()
                {
                    Success = false,
                    Message = "Username already eixsted",
                    Errors = ["Username already eixsted"]
                };
            }

            // create new user
            var user = new ApplicationUser { Email = request.Username, UserName = request.Username };
            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            // create new user fail
            if (!createUserResult.Succeeded)
            {
                return new BaseResponse<string>()
                {
                    Success = false,
                    Message = "Create user failed",
                    Errors = createUserResult.Errors.Select(e => e.Description).ToList()
                };
            }

            // add role to user
            foreach (var role in request.Roles)
            {
                await _userManager.AddToRoleAsync(user, role.ToString());
            }

            // if register by email
            if (IsEmail(request.Username))
            {
                // generate new confirmation token and encode it
                var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(user)));

                var queryParams = new Dictionary<string, string>()
            {
                { "userId", user.Id },
                { "email", user.Email},
                { "code", token },
            };
                var mailConfirmationEndpoint = _configuration.GetValue<string>("MailConfirmationUrl") ?? throw new NotSupportedException("MailConfirmationUrl is not existed");
                var confirmEmailUrl = QueryHelpers.AddQueryString(mailConfirmationEndpoint ?? "", queryParams);

                // send email
                try
                {
                    await _emailService.SendConfirmMailAsync(user.Email, confirmEmailUrl);
                }
                catch
                {
                    new BaseResponse<string>()
                    {
                        Success = false,
                        Message = "There is some problem at mail service, please contact with admin at (+84)9276122811",
                    };
                }
            }
            return new BaseResponse<string>()
            {
                Success = true,
                Message = "User registered successfully, Please click link on your email to complete registration process"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>()
            {
                Success = false,
                Message = ex.Message,
            };
        }
    }
    bool IsEmail(string email)
    {
        // Simple regex pattern for validating email addresses
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}