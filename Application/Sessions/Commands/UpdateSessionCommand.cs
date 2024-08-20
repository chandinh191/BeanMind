using Application.Common;
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
using System.Text;
using System.Threading.Tasks;

namespace Application.Sessions.Commands
{
    [AutoMap(typeof(Domain.Entities.Session), ReverseMap = true)]
    public sealed record UpdateSessionCommand : IRequest<BaseResponse<GetBriefSessionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string LecturerId { get; set; }
        [Required]
        public Guid TeachingSlotId { get; set; }
    }

    public class UpdateSessionCommandHanler : IRequestHandler<UpdateSessionCommand, BaseResponse<GetBriefSessionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateSessionCommandHanler(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<GetBriefSessionResponseModel>> Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (session == null)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "Session is not found",
                    Errors = ["Session is not found"]
                };
            }

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
                    .FirstOrDefaultAsync(x => x.ApplicationUserId == request.LecturerId && x.CourseId == teachingSlot.CourseId && x.IsDeleted==false);
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
            }
*/
/*            var checkDuplicateTime = _context.Sessions
                .Where(o => o.ApplicationUserId == request.LecturerId
                && o.Date == request.Date) //trùng ngày
                .AsQueryable();
            if (checkDuplicateTime != null && checkDuplicateTime.Count() > 0) //Nếu có lịch dạy trùng thời gian 
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "The lecturer have a session during this time",
                };
            }*/

            //_mapper.Map(request, question);
            // Use reflection to update non-null properties
            if(request.LecturerId != null)
            {
                session.ApplicationUserId = request.LecturerId;
            }
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = session.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(session, requestValue);
                    }
                }
            }

            var updateSessionResult = _context.Update(session);

            if (updateSessionResult.Entity == null)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "Update session failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedSessionResult = _mapper.Map<GetBriefSessionResponseModel>(updateSessionResult.Entity);

            return new BaseResponse<GetBriefSessionResponseModel>
            {
                Success = true,
                Message = "Update session successful",
                Data = mappedSessionResult
            };
        }
    }
}
