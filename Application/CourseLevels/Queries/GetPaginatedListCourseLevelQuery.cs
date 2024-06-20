using Application.Common;
using AutoMapper;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CourseLevels.Queries
{
    public sealed record GetPaginatedListCourseLevelQuery : IRequest<BaseResponse<Pagination<GetBriefCourseLevelResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string? Term { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListCourseLevelQueryHandler : IRequestHandler<GetPaginatedListCourseLevelQuery, BaseResponse<Pagination<GetBriefCourseLevelResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListCourseLevelQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefCourseLevelResponseModel>>> Handle(GetPaginatedListCourseLevelQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var programTypes = _context.CourseLevel.AsQueryable();

            // filter by search Title and description
            if (!string.IsNullOrEmpty(request.Term))
            {
                programTypes = programTypes.Where(x => x.Title.Contains(request.Term) || x.Description.Contains(request.Term));
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                programTypes = programTypes.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                programTypes = programTypes.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                programTypes = programTypes.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                programTypes = programTypes.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                programTypes = programTypes.Where(course =>
                    course.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                programTypes = programTypes.Where(course =>
                    course.Created <= request.EndTime);
            }
            // convert the list of item to list of response model
            var mappedProgramTypes = _mapper.Map<List<GetBriefCourseLevelResponseModel>>(programTypes);
            var createPaginatedListResult = Pagination<GetBriefCourseLevelResponseModel>.Create(mappedProgramTypes.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefCourseLevelResponseModel>>
                {
                    Success = false,
                    Message = "Get PaginatedList program type failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefCourseLevelResponseModel>>
            {
                Success = true,
                Message = "Get PaginatedList program type successful",
                Data = createPaginatedListResult,
            };
        }
    }

}
