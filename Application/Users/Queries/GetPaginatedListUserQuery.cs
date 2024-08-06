using Application.Common;
using Application.Courses;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public sealed record GetPaginatedListUserQuery : IRequest<BaseResponse<Pagination<GetUserInfoResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string? Term { get; init; }
        public string? Role { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;

    }

    public class GetPaginatedListUserQueryHandler : IRequestHandler<GetPaginatedListUserQuery, BaseResponse<Pagination<GetUserInfoResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetPaginatedListUserQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<Pagination<GetUserInfoResponseModel>>> Handle(GetPaginatedListUserQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var users = _context.ApplicationUsers.AsQueryable();

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                users = users.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                users = users.Where(x => x.IsDeleted == false);
            }

            // filter by role
            if (request.Role != null)
            {
                var userIdsInRole = await _userManager.GetUsersInRoleAsync(request.Role);
                var userIds = userIdsInRole.Select(u => u.Id).ToList();
                users = users.Where(x => userIds.Contains(x.Id));
            }

            // convert the list of item to list of response model
            var mappedUsers = _mapper.Map<List<GetUserInfoResponseModel>>(users);
            var createPaginatedListResult = Pagination<GetUserInfoResponseModel>.Create(mappedUsers.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetUserInfoResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list user failed",
                };
            }
            foreach (var mappedUser in mappedUsers)
            {
                var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == mappedUser.Id);
                var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
                mappedUser.Roles = userRoles;
            }

            return new BaseResponse<Pagination<GetUserInfoResponseModel>>
            {
                Success = true,
                Message = "Get paginated list user successful",
                Data = createPaginatedListResult,
            };
        }
    }

}
