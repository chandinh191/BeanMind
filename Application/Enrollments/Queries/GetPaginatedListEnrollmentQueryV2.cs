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

    public sealed record GetPaginatedListEnrollmentQueryV2 : IRequest<BaseResponse<Pagination<GetBriefEnrollmentResponseModelVer2>>>
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

    public class GetPaginatedListEnrollmentQueryV2Handler : IRequestHandler<GetPaginatedListEnrollmentQueryV2, BaseResponse<Pagination<GetBriefEnrollmentResponseModelVer2>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListEnrollmentQueryV2Handler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefEnrollmentResponseModelVer2>>> Handle(GetPaginatedListEnrollmentQueryV2 request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var enrollments = _context.Enrollments
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
            var mappedEnrollments = _mapper.Map<List<GetBriefEnrollmentResponseModelVer2>>(enrollments);
            foreach (var mappedEnrollment in mappedEnrollments)
            {
                mappedEnrollment.PercentTopicCompletion = CactulatePercentTopicCompletion(mappedEnrollment.Id, mappedEnrollment.CourseId);
            }

            var createPaginatedListResult = Pagination<GetBriefEnrollmentResponseModelVer2>.Create(mappedEnrollments.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefEnrollmentResponseModelVer2>>
                {
                    Success = false,
                    Message = "Get PaginatedList enrollment failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefEnrollmentResponseModelVer2>>
            {
                Success = true,
                Message = "Get PaginatedList enrollment successful",
                Data = createPaginatedListResult,
            };
        }
        public double CactulatePercentTopicCompletion(Guid enrollmentId, Guid courseId)
        {
            var processions = _context.Processions
                .Include(o => o.Participant).ThenInclude(o => o.Enrollment)
                //.Where(o => o.Participant.IsPresent == true && o.Participant.Status == Domain.Enums.ParticipantStatus.Done)
                .Where(o => o.Participant.Enrollment.Id == enrollmentId)
                .ToList();
            var topics = _context.Topics
               .Include(o => o.Chapter).ThenInclude(o => o.Course)
               .Where(o => o.Chapter.Course.Id == courseId)
               .ToList();
            return ((double)processions.Count() / topics.Count()) * 100;
        }
    }


}
