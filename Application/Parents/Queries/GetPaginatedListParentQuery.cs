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

namespace Application.Parents.Queries
{
    public sealed record GetPaginatedListParentQuery : IRequest<BaseResponse<Pagination<GetBriefParentResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string? ApplicationUserId { get; set; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListParentQueryHandler : IRequestHandler<GetPaginatedListParentQuery, BaseResponse<Pagination<GetBriefParentResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListParentQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefParentResponseModel>>> Handle(GetPaginatedListParentQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var parents = _context.Parents.AsQueryable();

            if (request.ApplicationUserId != null)
            {
                parents = parents.Where(x => x.ApplicationUserId == request.ApplicationUserId);
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                parents = parents.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                parents = parents.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                parents = parents.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                parents = parents.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                parents = parents.Where(participant =>
                    participant.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                parents = parents.Where(participant =>
                     participant.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedParents = _mapper.Map<List<GetBriefParentResponseModel>>(parents);
            var createPaginatedListResult = Pagination<GetBriefParentResponseModel>.Create(mappedParents.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefParentResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list parent failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefParentResponseModel>>
            {
                Success = true,
                Message = "Get paginated list parent successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
