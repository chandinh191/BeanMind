using Application.Common;
using Application.Orders;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public sealed record GetPaginatedListOrderQuery : IRequest<BaseResponse<Pagination<GetBriefOrderResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string? ApplicationUserId { get; set; }
        public Guid? CourseId { get; set; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListParentQueryHandler : IRequestHandler<GetPaginatedListOrderQuery, BaseResponse<Pagination<GetBriefOrderResponseModel>>>
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

        public async Task<BaseResponse<Pagination<GetBriefOrderResponseModel>>> Handle(GetPaginatedListOrderQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var orders = _context.Orders
                                .Include(o => o.ApplicationUser)
                .Include(o => o.Course)
                .AsQueryable();

            if (request.ApplicationUserId != null)
            {
                orders = orders.Where(x => x.ApplicationUserId == request.ApplicationUserId);
            }
            if (request.CourseId != null)
            {
                orders = orders.Where(x => x.CourseId == request.CourseId);
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                orders = orders.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                orders = orders.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                orders = orders.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                orders = orders.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                orders = orders.Where(participant =>
                    participant.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                orders = orders.Where(participant =>
                     participant.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedOrders = _mapper.Map<List<GetBriefOrderResponseModel>>(orders);
            var createPaginatedListResult = Pagination<GetBriefOrderResponseModel>.Create(mappedOrders.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefOrderResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list order failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefOrderResponseModel>>
            {
                Success = true,
                Message = "Get paginated list order successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
