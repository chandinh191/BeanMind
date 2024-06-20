using Application.Common;
using Infrastructure.Common.Email;
using Infrastructure.Services;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Configuration;

namespace Application.Users.Commands;

public record class ResendConfirmEmailCommand : IRequest<BaseResponse<string>>
{
    public required string Email { get; init; }
}

public class ResendConfirmEmailCommandHandler : IRequestHandler<ResendConfirmEmailCommand, BaseResponse<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;
    private readonly LinkGenerator _linkGenerator;
    private readonly IEmailService _emailService;

    public ResendConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, LinkGenerator linkGenerator, IEmailService emailService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _linkGenerator = linkGenerator;
        _emailService = emailService;
    }

    public async Task<BaseResponse<string>> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        // user not found
        if (user is null)
        {
            return new BaseResponse<string> { Success = false, Message = "User is not existed" };
        }

        // generate new confirmation token and encode it
        var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(user)));

        //// build route query by RouteValueDictionary
        //var routeValues = new RouteValueDictionary()
        //{
        //    ["userId"] = user.Id,
        //    ["code"] = token
        //};

        //// build confirm email url
        //var confirmEmailEndpointName = RouteNameValues.ConfirmEmail;
        //var confirmEmailUrl = _linkGenerator.GetUriByName(_contextAccessor.HttpContext, confirmEmailEndpointName, routeValues)
        //    ?? throw new NotSupportedException($"Could not find endpoint named '{confirmEmailEndpointName}'.");

        var queryParams = new Dictionary<string, string>()
        {
            { "userId", user.Id },
            { "email", token },
            { "code", token },
        };
        var mailConfirmationEndpoint = _configuration.GetValue<string>("MailConfirmationUrl") ?? throw new NotSupportedException("MailConfirmationUrl is not existed");
        var confirmEmailUrl = QueryHelpers.AddQueryString(mailConfirmationEndpoint ?? "" , queryParams);

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
            Message = "Email confirmation resent, Please click link on your email to complete registration process"
        };
    }
}