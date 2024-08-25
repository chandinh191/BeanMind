using Application.Common;
using AutoMapper;
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

namespace Application.Courses.Queries
{
    public sealed record GetPaginatedListHotCourseQuery : IRequest<BaseResponse<Pagination<GetCourseResponseModelVer2>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid SubjectId { get; init; }
        public Guid ProgramTypeId { get; init; }
        public Guid CourseLevelId { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListHotCourseQueryHandler : IRequestHandler<GetPaginatedListHotCourseQuery, BaseResponse<Pagination<GetCourseResponseModelVer2>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListHotCourseQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetCourseResponseModelVer2>>> Handle(GetPaginatedListHotCourseQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var courses = _context.Courses
                .AsQueryable();

            // filter by subject id
            if (request.SubjectId != Guid.Empty)
            {
                courses = courses.Where(x => x.SubjectId == request.SubjectId);
            }

            // filter by ProgramType Id
            if (request.ProgramTypeId != Guid.Empty)
            {
                courses = courses.Where(x => x.ProgramTypeId == request.ProgramTypeId);
            }

            // filter by CourseLevel Id
            if (request.CourseLevelId != Guid.Empty)
            {
                courses = courses.Where(x => x.CourseLevelId == request.CourseLevelId);
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                courses = courses.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                courses = courses.Where(x => x.IsDeleted == false);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                courses = courses.Where(o => o.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                courses = courses.Where(o => o.Created <= request.EndTime);
            }



            // convert the list of item to list of response model
            var mappedCourses = _mapper.Map<List<GetCourseResponseModelVer2>>(courses);
            foreach (var mappedCourse in mappedCourses)
            {
                var session = _context.Sessions
                  .Include(o => o.TeachingSlot)
                  .Where(o => o.Date >= DateTime.Now.AddHours(14))
                  .Where(o => o.TeachingSlot.CourseId == mappedCourse.Id)
                  .AsQueryable();
                int futureSession = session.Count();
                if (futureSession >= mappedCourse.TotalSlot)
                {
                    mappedCourse.IsAvailable = true;
                }
                var enrolment = _context.Enrollments
                 .Where(o => o.CourseId == mappedCourse.Id && o.IsDeleted == false)
                 .AsQueryable();
                mappedCourse.NumberOfEnrollment = enrolment.Count();
            }

            // Filter by enrollment count and order by descending
            mappedCourses = mappedCourses.OrderByDescending(x => x.NumberOfEnrollment).ToList();

            var createPaginatedListResult = Pagination<GetCourseResponseModelVer2>.Create(mappedCourses.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetCourseResponseModelVer2>>
                {
                    Success = false,
                    Message = "Get paginated list course failed",
                };
            }

            return new BaseResponse<Pagination<GetCourseResponseModelVer2>>
            {
                Success = true,
                Message = "Get paginated list course successfully",
                Data = createPaginatedListResult,
            };
        }
    }

}
