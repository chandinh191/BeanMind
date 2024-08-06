using Application.Common;
using Application.TeachingSlots;
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

namespace Application.TeachingSlots.Queries
{
    public sealed record GetPaginatedListTeachingSlotQuery : IRequest<BaseResponse<Pagination<GetBriefTeachingSlotResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string? Term { get; init; }
        public Guid CourseId { get; set; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListTeachingSlotQueryHandler : IRequestHandler<GetPaginatedListTeachingSlotQuery, BaseResponse<Pagination<GetBriefTeachingSlotResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListTeachingSlotQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefTeachingSlotResponseModel>>> Handle(GetPaginatedListTeachingSlotQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var teachingSlots = _context.TeachingSlots
                .Include(o => o.Course)
                .AsQueryable();

            if (request.CourseId != Guid.Empty)
            {
                teachingSlots = teachingSlots.Where(x => x.CourseId == request.CourseId);
            }

            // filter by isDeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                teachingSlots = teachingSlots.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                teachingSlots = teachingSlots.Where(x => x.IsDeleted == false);
            }

            // filter by filterDate
            if (request.SortBy == SortBy.Ascending)
            {
                teachingSlots = teachingSlots.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                teachingSlots = teachingSlots.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                teachingSlots = teachingSlots.Where(o =>
                    o.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                teachingSlots = teachingSlots.Where(o =>
                    o.Created <= request.EndTime);
            }
            // filter by search Title and description
            if (!string.IsNullOrEmpty(request.Term))
            {
                teachingSlots = teachingSlots.Where(x => x.Title.Contains(request.Term));
            }
            // convert the list of item to list of response model
            var mappedTeachingSlots = _mapper.Map<List<GetBriefTeachingSlotResponseModel>>(teachingSlots);
            var createPaginatedListResult = Pagination<GetBriefTeachingSlotResponseModel>.Create(mappedTeachingSlots.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefTeachingSlotResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list teaching slot failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefTeachingSlotResponseModel>>
            {
                Success = true,
                Message = "Get  paginated list teaching slot successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
