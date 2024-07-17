using Application.Common;
using Application.CourseLevels;
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

namespace Application.CourseLevels.Commands
{

    [AutoMap(typeof(Domain.Entities.CourseLevel), ReverseMap = true)]
    public sealed record UpdateCourseLevelCommand : IRequest<BaseResponse<GetBriefCourseLevelResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
        public string? Title { get; init; }
        public string? Description { get; init; }
    }

    public class UpdateCourseLevelCommandHanler : IRequestHandler<UpdateCourseLevelCommand, BaseResponse<GetBriefCourseLevelResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCourseLevelCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefCourseLevelResponseModel>> Handle(UpdateCourseLevelCommand request, CancellationToken cancellationToken)
        {

            var courseLevel = await _context.CourseLevels.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (courseLevel == null)
            {
                return new BaseResponse<GetBriefCourseLevelResponseModel>
                {
                    Success = false,
                    Message = "Course level is not found",
                    Errors = ["Course level is not found"]
                };
            }

            //_mapper.Map(request, courseLevel);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var courseProperty = courseLevel.GetType().GetProperty(property.Name);
                    if (courseProperty != null)
                    {
                        courseProperty.SetValue(courseLevel, requestValue);
                    }
                }
            }

            var updateCourseLevelResult = _context.Update(courseLevel);

            if (updateCourseLevelResult.Entity == null)
            {
                return new BaseResponse<GetBriefCourseLevelResponseModel>
                {
                    Success = false,
                    Message = "Update course level failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedCourseLevelResult = _mapper.Map<GetBriefCourseLevelResponseModel>(updateCourseLevelResult.Entity);

            return new BaseResponse<GetBriefCourseLevelResponseModel>
            {
                Success = true,
                Message = "Update course level successful",
                Data = mappedCourseLevelResult
            };
        }
    }

}
