using Application.Common;
using Application.Courses;
using AutoMapper;
using Domain.Entities;
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
            var users = _userManager.Users.ToArray();
     
            // convert the list of item to list of response model
            var mappedUsers = _mapper.Map<List<GetUserInfoResponseModel>>(users);
            var createPaginatedListResult = Pagination<GetUserInfoResponseModel>.Create(mappedUsers.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetUserInfoResponseModel>>
                {
                    Success = false,
                    Message = "Get PaginatedList user failed",
                };
            }

            return new BaseResponse<Pagination<GetUserInfoResponseModel>>
            {
                Success = true,
                Message = "Get PaginatedList course successful",
                Data = createPaginatedListResult,
            };
        }
    }

}
