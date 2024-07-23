using Application.Common;
using Application.ProgramTypes;
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

namespace Application.ProgramTypes.Queries
{
    public sealed record GetPaginatedListProgramTypeQuery : IRequest<BaseResponse<Pagination<GetBriefProgramTypeResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string? Term { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListProgramTypeQueryHandler : IRequestHandler<GetPaginatedListProgramTypeQuery, BaseResponse<Pagination<GetBriefProgramTypeResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListProgramTypeQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefProgramTypeResponseModel>>> Handle(GetPaginatedListProgramTypeQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var courselevels = _context.ProgramTypes.AsQueryable();

            // filter by search Title and description
            if (!string.IsNullOrEmpty(request.Term))
            {
                courselevels = courselevels.Where(x => x.Title.Contains(request.Term) || x.Description.Contains(request.Term));
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                courselevels = courselevels.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                courselevels = courselevels.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                courselevels = courselevels.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                courselevels = courselevels.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                courselevels = courselevels.Where(o =>
                    o.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                courselevels = courselevels.Where(o =>
                    o.Created <= request.EndTime);
            }
            // convert the list of item to list of response model
            var mappedCourses = _mapper.Map<List<GetBriefProgramTypeResponseModel>>(courselevels);
            var createPaginatedListResult = Pagination<GetBriefProgramTypeResponseModel>.Create(mappedCourses.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefProgramTypeResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list program type failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefProgramTypeResponseModel>>
            {
                Success = true,
                Message = "Get paginated list program type successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
