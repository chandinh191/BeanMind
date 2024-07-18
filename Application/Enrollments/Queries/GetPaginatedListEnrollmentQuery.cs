using Application.Enrollments;
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
using Microsoft.EntityFrameworkCore;

namespace Application.Enrollments.Queries
{

    public sealed record GetPaginatedListEnrollmentQuery : IRequest<BaseResponse<Pagination<GetBriefEnrollmentResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid CourseId { get; init; }
        public string? ApplicationUserId { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListEnrollmentQueryHandler : IRequestHandler<GetPaginatedListEnrollmentQuery, BaseResponse<Pagination<GetBriefEnrollmentResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListEnrollmentQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefEnrollmentResponseModel>>> Handle(GetPaginatedListEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var enrollments = _context.Enrollments
                .Include(o => o.Course)
                .Include(o => o.ApplicationUser)
                .AsQueryable();

            // filter by course id
            if (request.CourseId != Guid.Empty)
            {
                enrollments = enrollments.Where(x => x.CourseId == request.CourseId);
            }

            // filter by ApplicationUserId
            if (request.ApplicationUserId != null)
            {
                enrollments = enrollments.Where(x => x.ApplicationUserId.Equals(request.ApplicationUserId));
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                enrollments = enrollments.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                enrollments = enrollments.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                enrollments = enrollments.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                enrollments = enrollments.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                enrollments = enrollments.Where(o => o.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                enrollments = enrollments.Where(o => o.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedEnrollments = _mapper.Map<List<GetBriefEnrollmentResponseModel>>(enrollments);
            var createPaginatedListResult = Pagination<GetBriefEnrollmentResponseModel>.Create(mappedEnrollments.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefEnrollmentResponseModel>>
                {
                    Success = false,
                    Message = "Get PaginatedList enrollment failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefEnrollmentResponseModel>>
            {
                Success = true,
                Message = "Get PaginatedList enrollment successful",
                Data = createPaginatedListResult,
            };
        }
    }

}
