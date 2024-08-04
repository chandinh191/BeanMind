using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
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
        public DateOnly Date { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public Guid TeachingSlotId { get; set; }
    }

    public class CreateSessionCommandHanler : IRequestHandler<CreateSessionCommand, BaseResponse<GetBriefSessionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateSessionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefSessionResponseModel>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _context.ApplicationUsers
                .Include(o => o.Teacher)
                .FirstOrDefaultAsync(x => x.Id == request.ApplicationUserId);
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
                if(applicationUser.Teacher == null)
                {
                    return new BaseResponse<GetBriefSessionResponseModel>
                    {
                        Success = false,
                        Message = "Teacher not found",
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
                    .FirstOrDefaultAsync(x => x.ApplicationUserId == request.ApplicationUserId && x.CourseId == teachingSlot.CourseId && x.IsDeleted == false);
            if (teachable == null)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "User are not able to teach this course",
                };
            }
            int dayOfWeek = (int)request.Date.DayOfWeek;
            if (dayOfWeek != teachingSlot.DayInWeek)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "You need to create exactly what day of the week compared to your teaching slot data",
                };
            }

            var checkDuplicateTime =  _context.Sessions
                .Where(o => o.ApplicationUserId == request.ApplicationUserId 
                && o.TeachingSlot.Slot == teachingSlot.Slot //trùng slot cái đang tạo
                && o.Date == request.Date) //trùng ngày
                .AsQueryable();
            if (checkDuplicateTime != null && checkDuplicateTime.Count() > 0) //Nếu có lịch dạy trùng thời gian 
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "The lecturer have a session during this time",
                };
            }

            var session = _mapper.Map<Domain.Entities.Session>(request);
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
