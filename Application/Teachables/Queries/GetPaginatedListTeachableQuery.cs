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

namespace Application.Teachables.Queries
{
    public sealed record GetPaginatedListTeachableQuery : IRequest<BaseResponse<Pagination<GetBriefTeachableResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string ApplicationUserId { get; set; }
        public Guid CourseId { get; set; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListTeachableQueryHandler : IRequestHandler<GetPaginatedListTeachableQuery, BaseResponse<Pagination<GetBriefTeachableResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListTeachableQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefTeachableResponseModel>>> Handle(GetPaginatedListTeachableQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var teachables = _context.Teachables.AsQueryable();

            if (request.ApplicationUserId != null)
            {
                teachables = teachables.Where(x => x.ApplicationUserId == request.ApplicationUserId);
            }

            if (request.CourseId != Guid.Empty)
            {
                teachables = teachables.Where(x => x.CourseId == request.CourseId);
            }

            // filter by isDeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                teachables = teachables.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                teachables = teachables.Where(x => x.IsDeleted == false);
            }

            // filter by filterDate
            if (request.SortBy == SortBy.Ascending)
            {
                teachables = teachables.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                teachables = teachables.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                teachables = teachables.Where(o =>
                    o.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                teachables = teachables.Where(o =>
                    o.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedTeachables = _mapper.Map<List<GetBriefTeachableResponseModel>>(teachables);
            var createPaginatedListResult = Pagination<GetBriefTeachableResponseModel>.Create(mappedTeachables.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefTeachableResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list teachable failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefTeachableResponseModel>>
            {
                Success = true,
                Message = "Get  paginated list teachable successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
