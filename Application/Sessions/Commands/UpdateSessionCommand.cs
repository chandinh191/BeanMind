﻿using Application.Common;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
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
        public DateOnly Date { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public Guid TeachingSlotId { get; set; }
    }

    public class UpdateSessionCommandHanler : IRequestHandler<UpdateSessionCommand, BaseResponse<GetBriefSessionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateSessionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                if (applicationUser.Teacher == null)
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
                    .Where(o => o.Status == true)
                    .FirstOrDefaultAsync(x => x.ApplicationUserId == request.ApplicationUserId && x.CourseId == teachingSlot.CourseId);
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

            var checkDuplicateTime = _context.Sessions
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

            //_mapper.Map(request, question);
            // Use reflection to update non-null properties
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
