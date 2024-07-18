using Application.Common;
using Application.GameHistories;
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

namespace Application.GameHistories.Queries
{
    public sealed record GetPaginatedListGameHistoryQuery : IRequest<BaseResponse<Pagination<GetBriefGameHistoryResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid GameId { get; set; }
        public string? ApplicationUserId { get; set; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListGameHistoryQueryHandler : IRequestHandler<GetPaginatedListGameHistoryQuery, BaseResponse<Pagination<GetBriefGameHistoryResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListGameHistoryQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefGameHistoryResponseModel>>> Handle(GetPaginatedListGameHistoryQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var gameHistories = _context.GameHistories
                .AsQueryable();
            // filter by course id
            if (request.GameId != Guid.Empty)
            {
                gameHistories = gameHistories.Where(x => x.GameId == request.GameId);
            }

            // filter by ApplicationUserId
            if (request.ApplicationUserId != null)
            {
                gameHistories = gameHistories.Where(x => x.ApplicationUserId.Equals(request.ApplicationUserId));
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                gameHistories = gameHistories.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                gameHistories = gameHistories.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                gameHistories = gameHistories.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                gameHistories = gameHistories.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                gameHistories = gameHistories.Where(o => o.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                gameHistories = gameHistories.Where(o => o.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedGameHistories = _mapper.Map<List<GetBriefGameHistoryResponseModel>>(gameHistories);
            var createPaginatedListResult = Pagination<GetBriefGameHistoryResponseModel>.Create(mappedGameHistories.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefGameHistoryResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list game history failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefGameHistoryResponseModel>>
            {
                Success = true,
                Message = "Get paginated list game history successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
