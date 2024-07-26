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
    public sealed record CreateTeachingSlotCommand : IRequest<BaseResponse<GetBriefTeachingSlotResponseModel>>
    {
        public string Title { get; set; }
        public int DayInWeek { get; set; }
        public int Slot { get; set; }
        [Required]
        public Guid CourseId { get; set; }
    }

    public class CreateTeachingSlotCommandHanler : IRequestHandler<CreateTeachingSlotCommand, BaseResponse<GetBriefTeachingSlotResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTeachingSlotCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefTeachingSlotResponseModel>> Handle(CreateTeachingSlotCommand request, CancellationToken cancellationToken)
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
            var teachingSlot = _mapper.Map<Domain.Entities.TeachingSlot>(request);
            var createTeachingSlotResult = await _context.AddAsync(teachingSlot, cancellationToken);

            if (createTeachingSlotResult.Entity == null)
            {
                return new BaseResponse<GetBriefTeachingSlotResponseModel>
                {
                    Success = false,
                    Message = "Create teaching slot failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTeachingSlotResult = _mapper.Map<GetBriefTeachingSlotResponseModel>(createTeachingSlotResult.Entity);

            return new BaseResponse<GetBriefTeachingSlotResponseModel>
            {
                Success = true,
                Message = "Create teaching slot successful",
                Data = mappedTeachingSlotResult
            };
        }
    }
}
