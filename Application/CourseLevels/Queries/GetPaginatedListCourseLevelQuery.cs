using Application.Common;
using AutoMapper;
using Domain.Entities;
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
            var courseLevels = _context.CourseLevels.AsQueryable();

            // filter by search Title and description
            if (!string.IsNullOrEmpty(request.Term))
            {
                courseLevels = courseLevels.Where(x => x.Title.Contains(request.Term) || x.Description.Contains(request.Term));
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                courseLevels = courseLevels.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                courseLevels = courseLevels.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                courseLevels = courseLevels.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                courseLevels = courseLevels.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                courseLevels = courseLevels.Where(o => o.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                courseLevels = courseLevels.Where(o => o.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedCourseLevels = _mapper.Map<List<GetBriefCourseLevelResponseModel>>(courseLevels);
            var createPaginatedListResult = Pagination<GetBriefCourseLevelResponseModel>.Create(mappedCourseLevels.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefCourseLevelResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list course level failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefCourseLevelResponseModel>>
            {
                Success = true,
                Message = "Get paginated list course level successful",
                Data = createPaginatedListResult,
            };
        }
    }

}
