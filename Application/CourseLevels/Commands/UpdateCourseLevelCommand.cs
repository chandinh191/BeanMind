using Application.Common;
using Application.CourseLevels;
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

namespace Application.CourseLevels.Commands
{

    [AutoMap(typeof(Domain.Entities.CourseLevel), ReverseMap = true)]
    public sealed record UpdateCourseLevelCommand : IRequest<BaseResponse<GetCourseLevelResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
        public string? Title { get; init; }
        [Required]
        public string? Description { get; init; }
    }

    public class UpdateCourseLevelCommandHanler : IRequestHandler<UpdateCourseLevelCommand, BaseResponse<GetCourseLevelResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCourseLevelCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetCourseLevelResponseModel>> Handle(UpdateCourseLevelCommand request, CancellationToken cancellationToken)
        {

            var courseLevel = await _context.CourseLevels.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (courseLevel == null)
            {
                return new BaseResponse<GetCourseLevelResponseModel>
                {
                    Success = false,
                    Message = "Course level is not found",
                    Errors = ["Course level is not found"]
                };
            }

            _mapper.Map(request, courseLevel);

            var updateCourseLevelResult = _context.Update(courseLevel);

            if (updateCourseLevelResult.Entity == null)
            {
                return new BaseResponse<GetCourseLevelResponseModel>
                {
                    Success = false,
                    Message = "Update course level failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedCourseLevelResult = _mapper.Map<GetCourseLevelResponseModel>(updateCourseLevelResult.Entity);

            return new BaseResponse<GetCourseLevelResponseModel>
            {
                Success = true,
                Message = "Update course level successful",
                Data = mappedCourseLevelResult
            };
        }
    }

}
