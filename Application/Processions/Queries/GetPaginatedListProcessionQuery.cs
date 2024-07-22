using Application.Common;
using Application.Processions;
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

namespace Application.Processions.Queries
{
    public sealed record GetPaginatedListProcessionQuery : IRequest<BaseResponse<Pagination<GetBriefProcessionResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid ParticipantId { get; set; }
        public Guid TopicId { get; set; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListProcessionQueryHandler : IRequestHandler<GetPaginatedListProcessionQuery, BaseResponse<Pagination<GetBriefProcessionResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListProcessionQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefProcessionResponseModel>>> Handle(GetPaginatedListProcessionQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var processions = _context.Processions.AsQueryable();

            // filter by ParticipantId 
            if (request.ParticipantId != Guid.Empty)
            {
                processions = processions.Where(x => x.ParticipantId == request.ParticipantId);
            }
            // filter by TopicId
            if (request.TopicId != Guid.Empty)
            {
                processions = processions.Where(x => x.TopicId == request.TopicId);
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                processions = processions.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                processions = processions.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                processions = processions.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                processions = processions.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                processions = processions.Where(o =>
                    o.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                processions = processions.Where(o =>
                     o.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedProcessions = _mapper.Map<List<GetBriefProcessionResponseModel>>(processions);
            var createPaginatedListResult = Pagination<GetBriefProcessionResponseModel>.Create(mappedProcessions.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefProcessionResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list procession failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefProcessionResponseModel>>
            {
                Success = true,
                Message = "Get paginated list procession successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
