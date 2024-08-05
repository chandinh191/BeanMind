using Application.Common;
using Application.Sessions;
using Application.TeachingSlots;
using AutoMapper;
using Domain.Entities.UserEntities;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public sealed record GetPaginatedListSessionByUserIdQuery : IRequest<BaseResponse<List<GetBriefSessionResponseModel>>>
    {
        [Required]
        public string Id { get; init; }
    }

    public class GetPaginatedListSessionByUserIdQueryHandler : IRequestHandler<GetPaginatedListSessionByUserIdQuery, BaseResponse<List<GetBriefSessionResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetPaginatedListSessionByUserIdQueryHandler(UserManager<ApplicationUser> userManager,ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<List<GetBriefSessionResponseModel>>> Handle(GetPaginatedListSessionByUserIdQuery request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userManager.FindByIdAsync(request.Id);
            if (applicationUser == null)
            {
                return new BaseResponse<List<GetBriefSessionResponseModel>>
                {
                    Success = false,
                    Message = "User not found",
                };
            }
            else
            {
                var isTeacher = await _userManager.IsInRoleAsync(applicationUser, "Teacher");
                if (isTeacher)
                {
                    var sessions = _context.Sessions
                    .Where(o => o.IsDeleted == false)
                    .Include(o => o.TeachingSlot)
                    .Include(o => o.ApplicationUser)
                    .Where(o => o.ApplicationUserId == applicationUser.Id)
                    .ToList();

                    // convert the list of item to list of response model
                    var mappedSessions = _mapper.Map<List<GetBriefSessionResponseModel>>(sessions);
                    if (mappedSessions == null)
                    {
                        return new BaseResponse<List<GetBriefSessionResponseModel>>
                        {
                            Success = false,
                            Message = "Get paginated list session failed",
                        };
                    }
                    return new BaseResponse<List<GetBriefSessionResponseModel>>
                    {
                        Success = true,
                        Message = "Get  paginated list session successful",
                        Data = mappedSessions,
                    };
                }
                var isStudent = await _userManager.IsInRoleAsync(applicationUser, "Student");
                if (isStudent)
                {
                    var sessions = _context.Sessions
                    .Where(o => o.IsDeleted == false)
                    .Include(o => o.Participants).ThenInclude(o => o.Enrollment)
                    .Where(o => o.Participants.Any(e => e.Enrollment.ApplicationUserId == request.Id && e.Enrollment.Status == EnrollmentStatus.OnGoing && e.Enrollment.IsDeleted == false))
                    .ToList();

                    // convert the list of item to list of response model
                    var mappedSessions = _mapper.Map<List<GetBriefSessionResponseModel>>(sessions);
                    if (mappedSessions == null)
                    {
                        return new BaseResponse<List<GetBriefSessionResponseModel>>
                        {
                            Success = false,
                            Message = "Get paginated list session failed",
                        };
                    }
                    return new BaseResponse<List<GetBriefSessionResponseModel>>
                    {
                        Success = true,
                        Message = "Get paginated list session successful",
                        Data = mappedSessions,
                    };
                }
                return new BaseResponse<List<GetBriefSessionResponseModel>>
                {
                    Success = false,
                    Message = "User do not have any teacher role or student role",
                };
            }
        }
    }
}
