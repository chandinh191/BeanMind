using Application.Common;
using AutoMapper;
using Domain.Entities.UserEntities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationUsers.Queries
{
    public record class GetApplicationUserQuery : IRequest<BaseResponse<GetApplicationUserResponseModel>>
    {
        public required string UserId { get; init; }
    }

    public class GetApplicationUserQueryHandler : IRequestHandler<GetApplicationUserQuery, BaseResponse<GetApplicationUserResponseModel>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public GetApplicationUserQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<BaseResponse<GetApplicationUserResponseModel>> Handle(GetApplicationUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.ApplicationUsers
                .Include(o => o.Teachables)
                .Include(o => o.Sessions).ThenInclude(o => o.TeachingSlot)
                .Include(o => o.Enrollments).ThenInclude(o => o.WorksheetAttempts).ThenInclude(o => o.Worksheet)
                .Include(x => x.Parent)
                .Include(x => x.Student)
                .Include(x => x.Teacher)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.UserId), cancellationToken);
            // user not found
            if (user is null)
            {
                return new BaseResponse<GetApplicationUserResponseModel> { Success = false, Message = "User is not existed" };
            }

            var userResponse = _mapper.Map<GetApplicationUserResponseModel>(user);

            return new BaseResponse<GetApplicationUserResponseModel>
            {
                Success = true,
                Message = "Get user info successfully",
                Data = userResponse,
            };
        }
    }

}
