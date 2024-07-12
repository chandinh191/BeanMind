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

namespace Application.CourseLevels.Queries
{
    public sealed record GetCourseLevelQuery : IRequest<BaseResponse<GetCourseLevelResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetCourseLevelQueryHanler : IRequestHandler<GetCourseLevelQuery, BaseResponse<GetCourseLevelResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCourseLevelQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetCourseLevelResponseModel>> Handle(GetCourseLevelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetCourseLevelResponseModel>
                {
                    Success = false,
                    Message = "Get course level failed",
                    Errors = ["Id required"],
                };
            }

            var course = await _context.CourseLevels
                .Include(o=>o.Courses)
                .Include (o=>o.Teachables)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedCourse = _mapper.Map<GetCourseLevelResponseModel>(course);

            return new BaseResponse<GetCourseLevelResponseModel>
            {
                Success = true,
                Message = "Get course level successful",
                Data = mappedCourse
            };
        }
    }

}
