using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Courses.Queries;

public sealed record GetCourseQuery : IRequest<BaseResponse<GetCourseResponseModelVer2>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetCourseQueryHanler : IRequestHandler<GetCourseQuery, BaseResponse<GetCourseResponseModelVer2>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCourseQueryHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetCourseResponseModelVer2>> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            return new BaseResponse<GetCourseResponseModelVer2>
            {
                Success = false,
                Message = "Get course failed",
                Errors = ["Id required"],
            };
        }

        var course = await _context.Courses
            .Include(x => x.Subject)
            .Include(x => x.Chapters).ThenInclude(x => x.Topics)
            .Include(x => x.ProgramType)
            .Include(x => x.CourseLevel)
            .Include(x => x.Teachables).ThenInclude(o => o.ApplicationUser)
            .Include(o => o.WorksheetTemplates).ThenInclude(o => o.Worksheets)
            .Include(o => o.TeachingSlots)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

        var mappedCourse = _mapper.Map<GetCourseResponseModelVer2>(course);
        var enrolment = _context.Enrollments
             .Where(o => o.CourseId == mappedCourse.Id && o.IsDeleted == false)
             .AsQueryable();
        mappedCourse.NumberOfEnrollment = enrolment.Count();

        return new BaseResponse<GetCourseResponseModelVer2>
        {
            Success = true,
            Message = "Get course successful",
            Data = mappedCourse
        };
    }
}
