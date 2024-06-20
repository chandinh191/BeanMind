using Application.Common;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Application.Users.Commands;

public record class ConfirmEmailCommand : IRequest<BaseResponse<string>>
{
    public required string UserId { get; init; }
    public required string Code { get; init; }
}

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, BaseResponse<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<BaseResponse<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        // find user in database
        var userManager = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

        // user is not existed
        if(userManager is not { } user)
        {
            return new BaseResponse<string> { Success = false, Message = "User is not existed" };
        }

        // try decode url code
        var decodedToken = string.Empty;
        try
        {
            decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
        }
        catch (Exception)
        {
            return new BaseResponse<string> { Success = false, Message = "Token is not valid" };
        }

        // confirm email
        var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, decodedToken);

        // confirm email failed
        if(!confirmEmailResult.Succeeded)
        {
            return new BaseResponse<string>()
            {
                Success = false,
                Message = "Confirm email failed",
                Errors = confirmEmailResult.Errors.Select(e => e.Description).ToList()
            };
        }

        return new BaseResponse<string>()
        {
            Success = true,
            Message = "Confirm email successfully",
        };
    }
}
