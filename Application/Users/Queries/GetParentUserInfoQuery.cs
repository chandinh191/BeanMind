using Application.Common;
using Application.Parents;
using AutoMapper;
using Domain.Entities.UserEntities;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries
{

    public record class GetParentUserInfoQuery : IRequest<BaseResponse<GetParentUserInfoResponseModel>>
    {
        public required string UserId { get; init; }
    }

    public class GetParentUserInfoQueryHandler : IRequestHandler<GetParentUserInfoQuery, BaseResponse<GetParentUserInfoResponseModel>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public GetParentUserInfoQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<BaseResponse<GetParentUserInfoResponseModel>> Handle(GetParentUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.ApplicationUsers
                .Include(x => x.Parent)
                .ThenInclude(x=>x.Students)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.UserId), cancellationToken);
            // user not found
            if (user is null)
            {
                return new BaseResponse<GetParentUserInfoResponseModel> { Success = false, Message = "User is not existed" };
            }

            var parent = _context.Parents
                .Where(o => o.ApplicationUserId == request.UserId)
                .Include(o => o.Students)
                .FirstOrDefault();

            user.Parent = parent;
            user.ParentId = (parent == null) ? Guid.Empty : parent.Id;

            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();

            var userResponse = _mapper.Map<GetParentUserInfoResponseModel>(user);

            userResponse.Roles = userRoles;

            return new BaseResponse<GetParentUserInfoResponseModel>
            {
                Success = true,
                Message = "Get user info successfully",
                Data = userResponse,
            };
        }
    }

}
