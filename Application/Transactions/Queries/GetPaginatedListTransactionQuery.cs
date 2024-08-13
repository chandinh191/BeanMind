using Application.Common;
using Application.Topics;
using AutoMapper;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transactions.Queries
{
    public sealed record GetPaginatedListTransactionQuery : IRequest<BaseResponse<Pagination<GetBriefTransactionResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid OrderId { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListTopicQueryHandler : IRequestHandler<GetPaginatedListTransactionQuery, BaseResponse<Pagination<GetBriefTransactionResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListTopicQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefTransactionResponseModel>>> Handle(GetPaginatedListTransactionQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var transactions = _context.Transactions
                .Include(o => o.Order)
                .AsQueryable();

            // filter by ChapterId
            if (request.OrderId != Guid.Empty)
            {
                transactions = transactions.Where(x => x.OrderId == request.OrderId);
            }

            // filter by isDeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                transactions = transactions.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                transactions = transactions.Where(x => x.IsDeleted == false);
            }

            // filter by filterDate
            if (request.SortBy == SortBy.Ascending)
            {
                transactions = transactions.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                transactions = transactions.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                transactions = transactions.Where(topic =>
                    topic.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                transactions = transactions.Where(topic =>
                   topic.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedTransactions = _mapper.Map<List<GetBriefTransactionResponseModel>>(transactions);
            var createPaginatedListResult = Pagination<GetBriefTransactionResponseModel>.Create(mappedTransactions.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefTransactionResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list transaction failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefTransactionResponseModel>>
            {
                Success = true,
                Message = "Get paginated list transaction successful",
                Data = createPaginatedListResult,
            };
        }
    }

}
