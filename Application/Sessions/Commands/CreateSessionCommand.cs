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
            var teachingSlot = await _context.TeachingSlots.FirstOrDefaultAsync(x => x.Id == request.TeachingSlotId);
            if (teachingSlot == null)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "Teaching slot not found",
                };
            }
            var teachable = await _context.Teachables
                   .FirstOrDefaultAsync(x => x.ApplicationUserId == request.LecturerId && x.CourseId == teachingSlot.CourseId && x.IsDeleted == false);
            if (teachable == null)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "User are not able to teach this course",
                };
            }
            /*int dayOfWeek = (int)request.Date.DayOfWeek;
            if (dayOfWeek != teachingSlot.DayIndex)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "You need to create exactly what day of the week compared to your teaching slot data",
                };
            }*/
            //var session = _mapper.Map<Domain.Entities.Session>(request);
            var existedSession = _context.Sessions
              .Where(o => o.ApplicationUserId == request.LecturerId && o.TeachingSlotId == request.TeachingSlotId && o.Date == request.Date && o.IsDeleted ==false)
              .FirstOrDefault();
            if (existedSession != null)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "Session is existed",
                };
            }
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
