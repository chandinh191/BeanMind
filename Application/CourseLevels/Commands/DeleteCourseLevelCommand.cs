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
    public sealed record DeleteCourseLevelCommand : IRequest<BaseResponse<GetCourseLevelResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteCourseLevelCommandHanler : IRequestHandler<DeleteCourseLevelCommand, BaseResponse<GetCourseLevelResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteCourseLevelCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetCourseLevelResponseModel>> Handle(DeleteCourseLevelCommand request, CancellationToken cancellationToken)
        {
            var courseLevel = await _context.CourseLevel.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (courseLevel == null)
            {
                return new BaseResponse<GetCourseLevelResponseModel>
                {
                    Success = false,
                    Message = "Course level not found",
                };
            }
            courseLevel.IsDeleted = true;
            var updateCourseLevelResult = _context.Update(courseLevel);

            if (updateCourseLevelResult.Entity == null)
            {
                return new BaseResponse<GetCourseLevelResponseModel>
                {
                    Success = false,
                    Message = "Delete course level failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedCourseLevelResult = _mapper.Map<GetCourseLevelResponseModel>(updateCourseLevelResult.Entity);

            return new BaseResponse<GetCourseLevelResponseModel>
            {
                Success = true,
                Message = "Delete course level successful",
                Data = mappedCourseLevelResult
            };
        }
    }
}
