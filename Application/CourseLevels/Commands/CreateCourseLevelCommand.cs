using Application.Common;
using Application.CourseLevels;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CourseLevels.Commands
{
    [AutoMap(typeof(Domain.Entities.CourseLevel), ReverseMap = true)]
    public sealed record CreateCourseLevelCommand : IRequest<BaseResponse<GetBriefCourseLevelResponseModel>>
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
        public string Title { get; init; }
        public string Description { get; init; }

    }

    public class CreateCourseLevelCommandHanler : IRequestHandler<CreateCourseLevelCommand, BaseResponse<GetBriefCourseLevelResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCourseLevelCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefCourseLevelResponseModel>> Handle(CreateCourseLevelCommand request, CancellationToken cancellationToken)
        {
            var courseLevel = _mapper.Map<Domain.Entities.CourseLevel>(request);
            var createCourseLevelResult = await _context.AddAsync(courseLevel, cancellationToken);

            if (createCourseLevelResult.Entity == null)
            {
                return new BaseResponse<GetBriefCourseLevelResponseModel>
                {
                    Success = false,
                    Message = "Create course level failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedCourseLevelResult = _mapper.Map<GetBriefCourseLevelResponseModel>(createCourseLevelResult.Entity);

            return new BaseResponse<GetBriefCourseLevelResponseModel>
            {
                Success = true,
                Message = "Create courselevel successful",
                Data = mappedCourseLevelResult
            };
        }
    }

}
