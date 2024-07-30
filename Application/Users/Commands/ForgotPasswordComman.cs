using Application.Common;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using Domain.Entities.UserEntities;

namespace Application.Users.Commands;

public record class ForgotPasswordCommand : IRequest<BaseResponse<string>>
{
    public required string Email { get; init; }
}

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, BaseResponse<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailService emailService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<BaseResponse<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        // get user from db
        var user = await _userManager.FindByEmailAsync(request.Email);

        // user is not existed
        if(user is null)
        {
            return new BaseResponse<string> { Success = false, Message = "User is not existed" };
        }

        // user is not confirm email yet
        if(await _userManager.IsEmailConfirmedAsync(user) == false)
        {
            return new BaseResponse<string> { Success = false, Message = "User is not confirm Email yet" };
        }

        // create resetCode
        var resetCode = WebUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));
        var ResetPasswordEndpointHandler = _configuration.GetValue<string>("ResetPasswordUrl") ?? throw new NotSupportedException("MailConfirmationUrl is not existed");


        // send password reset email -> guide user to reset page
        try
        {
            await _emailService.SendPasswordResetCodeAsync(ResetPasswordEndpointHandler, user.Email, resetCode);
        }
        catch
        {
            return new BaseResponse<string> { Success = false, Message = "Send Email Failed" };
            throw;
        }

        return new BaseResponse<string>
        {
            Success = true,
            Message = "Send reset password mail successfully, Please follow the instruction to reset password"
        };
    }
}
