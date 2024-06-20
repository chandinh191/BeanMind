using MediatR;
using Application.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Routing;
using Infrastructure.Services;

namespace Application.Users.Commands;

public record class RegisterUserCommand : IRequest<BaseResponse<string>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
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
        // find user with email in database
        var existedUser = await _userManager.FindByEmailAsync(request.Email);

        // user with email already existed
        if (existedUser is not null)
        {
            return new BaseResponse<string>()
            {
                Success = false,
                Message = "Email already eixsted",
                Errors = ["Email already eixsted"]
            };
        }

        // create new user with hashed password (generated)
        var user = new ApplicationUser { Email = request.Email, UserName = request.Email };
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

        // add user to role
        // default role (for testing only) = admin, manager
        await _userManager.AddToRoleAsync(user, Domain.Constants.Roles.Manager);
        await _userManager.AddToRoleAsync(user, Domain.Constants.Roles.Administrator);

        // generate new confirmation token and encode it
        var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(user)));

        // build route query by RouteValueDictionary
        //var routeValues = new RouteValueDictionary()
        //{
        //    ["userId"] = user.Id,
        //    ["code"] = token
        //};

        // build confirm email url
        //var confirmEmailEndpointName = RouteNameValues.ConfirmEmail;
        //var confirmEmailUrl = _linkGenerator.GetUriByName(_contextAccessor.HttpContext, confirmEmailEndpointName, routeValues)
        //    ?? throw new NotSupportedException($"Could not find endpoint named '{confirmEmailEndpointName}'.");

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

        return new BaseResponse<string>()
        {
            Success = true,
            Message = "User registered successfully, Please click link on your email to complete registration process"
        };
    }
}