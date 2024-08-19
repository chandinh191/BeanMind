using Application.Common;
using Application.Enrollments;
using Application.Questions;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Sessions.Commands
{
    [AutoMap(typeof(Domain.Entities.Session), ReverseMap = true)]
    public sealed record CreateSessionCommand : IRequest<BaseResponse<GetBriefSessionResponseModel>>
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string LecturerId { get; set; }
        [Required]
        public Guid TeachingSlotId { get; set; }
    }

    public class CreateSessionCommandHanler : IRequestHandler<CreateSessionCommand, BaseResponse<GetBriefSessionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public CreateSessionCommandHanler(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<GetBriefSessionResponseModel>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userManager.FindByIdAsync(request.LecturerId);
            if (applicationUser == null)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "User not found",
                };
            }
            else
            {
                var isTeacher = await _userManager.IsInRoleAsync(applicationUser, "Teacher");
                if (!isTeacher)
                {
                    return new BaseResponse<GetBriefSessionResponseModel>
                    {
                        Success = false,
                        Message = "User is not a teacher to get this session",
                    };
                }
            }

            var teachingSlots = await _context.TeachingSlots
                                              .Include(ts => ts.Course)
                                              .ThenInclude(course => course.Teachables)
                                              .Where(ts => ts.Course.Teachables.Any(teachable => teachable.ApplicationUserId == request.LecturerId && teachable.IsDeleted == false))
                                              .Where(ts => ts.Id == request.TeachingSlotId)
                                              .ToListAsync();

            if (teachingSlots == null || teachingSlots.Count == 0)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "Teaching slot not found (filter from course for any Teachables of lecturer)",
                };
            }
            //var session = _mapper.Map<Domain.Entities.Session>(request);
            var session = new Session()
            {
                ApplicationUserId = request.LecturerId,
                TeachingSlotId = request.TeachingSlotId,
                Date = request.Date,
            };
            var createSessionResult = await _context.AddAsync(session, cancellationToken);

            if (createSessionResult.Entity == null)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "Create session failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);
            var mappedSessionResult = _mapper.Map<GetBriefSessionResponseModel>(createSessionResult.Entity);

            return new BaseResponse<GetBriefSessionResponseModel>
            {
                Success = true,
                Message = "Create session successful",
                Data = mappedSessionResult
            };
        }
    }
}
