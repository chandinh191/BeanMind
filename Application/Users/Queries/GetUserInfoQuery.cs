using Application.Common;
using AutoMapper;
using Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Queries;

public record class GetUserInfoQuery : IRequest<BaseResponse<GetUserInfoResponseModel>>
{
    public required string UserId { get; init; }
}

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, BaseResponse<GetUserInfoResponseModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public GetUserInfoQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetUserInfoResponseModel>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        // user not found
        if (user is null)
        {
            return new BaseResponse<GetUserInfoResponseModel> { Success = false, Message = "User is not existed" };
        }

        var userRoles = (await _userManager.GetRolesAsync(user)).ToList();

        var userResponse = _mapper.Map<GetUserInfoResponseModel>(user);
        userResponse.Roles = userRoles;

        return new BaseResponse<GetUserInfoResponseModel>
        {
            Success = true,
            Message = "Get user info successfully",
            Data = userResponse,
        };
    }
}
