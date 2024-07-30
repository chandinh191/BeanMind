using Application.Common;
using Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands;

public record class ResetPasswordCommand : IRequest<BaseResponse<string>>
{
    public required string Email { get; init; }
    public required string ResetCode { get; init; }
    public required string NewPassword { get; init; }
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, BaseResponse<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<BaseResponse<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        // find user in db
        var user = await _userManager.FindByEmailAsync(request.Email);

        // user is not existed
        if (user is null)
        {
            return new BaseResponse<string> { Success = false, Message = "User is not existed" };
        }

        // reset password
        var resetPasswordResult = await _userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);

        // reset password failed
        if (!resetPasswordResult.Succeeded)
        {
            return new BaseResponse<string>
            {
                Success = false,
                Message = "Reset password failed",
                Errors = resetPasswordResult.Errors.Select(e => e.Description).ToList()
            };
        }

        // invalidate all user token
        _ = await _userManager.UpdateSecurityStampAsync(user);

        return new BaseResponse<string>
        {
            Success = true,
            Message = "Password changed successfully",
        };
    }
}
