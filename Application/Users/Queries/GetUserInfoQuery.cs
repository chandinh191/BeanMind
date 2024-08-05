using Application.Common;
using AutoMapper;
using Domain.Entities.UserEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Application.Users.Queries;

public record class GetUserInfoQuery : IRequest<BaseResponse<GetUserInfoResponseModel>>
{
    public required string UserId { get; init; }
}

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, BaseResponse<GetUserInfoResponseModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public GetUserInfoQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    public async Task<BaseResponse<GetUserInfoResponseModel>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.ApplicationUsers
            .Include(x => x.Parent)
            .Include(x => x.Student)
            .Include(x => x.Teacher)
            .Include(x => x.Enrollments)
            .Include(x => x.Teachables)
            .Include(x => x.Sessions)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.UserId), cancellationToken);
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
