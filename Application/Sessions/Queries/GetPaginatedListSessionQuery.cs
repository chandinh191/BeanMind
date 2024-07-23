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

namespace Application.Sessions.Queries
{
    public sealed record GetPaginatedListSessionQuery : IRequest<BaseResponse<Pagination<GetBriefSessionResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string ApplicationUserId { get; set; }
        public Guid TeachingSlotId { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListSessionQueryHandler : IRequestHandler<GetPaginatedListSessionQuery, BaseResponse<Pagination<GetBriefSessionResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListSessionQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefSessionResponseModel>>> Handle(GetPaginatedListSessionQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var sessions = _context.Sessions.AsQueryable();

            if (request.ApplicationUserId != null)
            {
                sessions = sessions.Where(x => x.ApplicationUserId == request.ApplicationUserId);
            }

            if (request.TeachingSlotId != Guid.Empty)
            {
                sessions = sessions.Where(x => x.TeachingSlotId == request.TeachingSlotId);
            }

            // isdeleted filter
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                sessions = sessions.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                sessions = sessions.Where(x => x.IsDeleted == false);
            }

            // filter date Ascending or Descending
            if (request.SortBy == SortBy.Ascending)
            {
                sessions = sessions.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                sessions = sessions.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                sessions = sessions.Where(o =>
                    o.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                sessions = sessions.Where(o =>
                     o.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedSessions = _mapper.Map<List<GetBriefSessionResponseModel>>(sessions);
            var createPaginatedListResult = Pagination<GetBriefSessionResponseModel>.Create(mappedSessions.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefSessionResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list session failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefSessionResponseModel>>
            {
                Success = true,
                Message = "Get paginated list session successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
