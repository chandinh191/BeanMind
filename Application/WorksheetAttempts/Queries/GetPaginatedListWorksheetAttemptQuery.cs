using Application.Common;
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

namespace Application.WorksheetAttempts.Queries
{
    public sealed record GetPaginatedListWorksheetAttemptQuery : IRequest<BaseResponse<Pagination<GetBriefWorksheetAttemptResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid EnrollmentId { get; init; }
        public Guid WorksheetId { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListWorksheetAttemptQueryHandler : IRequestHandler<GetPaginatedListWorksheetAttemptQuery, BaseResponse<Pagination<GetBriefWorksheetAttemptResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListWorksheetAttemptQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefWorksheetAttemptResponseModel>>> Handle(GetPaginatedListWorksheetAttemptQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var worksheetAttempts = _context.WorksheetAttempts
                .Include(o => o.Enrollment)
                .Include(o => o.Worksheet)
                .AsQueryable();


            // filter by EnrollmentId
            if (request.EnrollmentId != Guid.Empty)
            {
                worksheetAttempts = worksheetAttempts.Where(x => x.EnrollmentId == request.EnrollmentId);
            }
            // filter by WorksheetId
            if (request.WorksheetId != Guid.Empty)
            {
                worksheetAttempts = worksheetAttempts.Where(x => x.WorksheetId == request.WorksheetId);
            }

            // filter by isDeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                worksheetAttempts = worksheetAttempts.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                worksheetAttempts = worksheetAttempts.Where(x => x.IsDeleted == false);
            }

            // filter by filterDate
            if (request.SortBy == SortBy.Ascending)
            {
                worksheetAttempts = worksheetAttempts.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                worksheetAttempts = worksheetAttempts.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                worksheetAttempts = worksheetAttempts.Where(topic =>
                    topic.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                worksheetAttempts = worksheetAttempts.Where(topic =>
                   topic.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedWorksheetAttempts = _mapper.Map<List<GetBriefWorksheetAttemptResponseModel>>(worksheetAttempts);
            var createPaginatedListResult = Pagination<GetBriefWorksheetAttemptResponseModel>.Create(mappedWorksheetAttempts.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefWorksheetAttemptResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list worksheet attempt failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefWorksheetAttemptResponseModel>>
            {
                Success = true,
                Message = "Get paginated list worksheet attempt successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
