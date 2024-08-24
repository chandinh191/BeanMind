using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeachingSlots.Commands
{
    [AutoMap(typeof(Domain.Entities.TeachingSlot), ReverseMap = true)]
    public sealed record UpdateTeachingSlotCommand : IRequest<BaseResponse<GetBriefTeachingSlotResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public int? DayIndex { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public Guid? CourseId { get; set; }
        public bool? IsDeleted { get; set; } 

    }

    public class UpdateTeachingSlotCommandHanler : IRequestHandler<UpdateTeachingSlotCommand, BaseResponse<GetBriefTeachingSlotResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateTeachingSlotCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefTeachingSlotResponseModel>> Handle(UpdateTeachingSlotCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId != null)
            {
                var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
                if (course == null)
                {
                    return new BaseResponse<GetBriefTeachingSlotResponseModel>
                    {
                        Success = false,
                        Message = "Course not found",
                    };
                }
            }

            var teachingSlot = await _context.TeachingSlots.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (teachingSlot == null)
            {
                return new BaseResponse<GetBriefTeachingSlotResponseModel>
                {
                    Success = false,
                    Message = "Teaching slot is not found",
                    Errors = ["Teaching slot is not found"]
                };
            }

            //_mapper.Map(request, question);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = teachingSlot.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(teachingSlot, requestValue);
                    }
                }
            }

            var updateTeachingSlotResult = _context.Update(teachingSlot);

            if (updateTeachingSlotResult.Entity == null)
            {
                return new BaseResponse<GetBriefTeachingSlotResponseModel>
                {
                    Success = false,
                    Message = "Update teaching slot failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTeachingSlotResult = _mapper.Map<GetBriefTeachingSlotResponseModel>(updateTeachingSlotResult.Entity);

            return new BaseResponse<GetBriefTeachingSlotResponseModel>
            {
                Success = true,
                Message = "Update teaching slot successful",
                Data = mappedTeachingSlotResult
            };
        }
    }
}
