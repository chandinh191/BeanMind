using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Application.Courses.Queries;

public sealed record GetPaginatedListCourseQuery : IRequest<BaseResponse<Pagination<GetBriefCourseResponseModel>>>
{
    public int PageIndex { get; init; }
    public int? PageSize { get; init; }
    public string? Term { get; init; }
    public Guid SubjectId { get; init; }
    public Guid ProgramTypeId { get; init; }
    public Guid CourseLevelId { get; init; }
    public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
    public SortBy SortBy { get; init; } 
    public DateTime StartTime { get; init; } = DateTime.MinValue;
    public DateTime EndTime { get; init; } = DateTime.MinValue;
}

public class GetPaginatedListCourseQueryHandler : IRequestHandler<GetPaginatedListCourseQuery, BaseResponse<Pagination<GetBriefCourseResponseModel>>>
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public GetPaginatedListCourseQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Pagination<GetBriefCourseResponseModel>>> Handle(GetPaginatedListCourseQuery request, CancellationToken cancellationToken)
    {
        var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
        var courses = _context.Courses
            .Include(o => o.ProgramType)
            .Include(o => o.CourseLevel)
            .Include(o => o.Subject)
            .Include(o => o.Teachables)
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

        // filter by search Title and description
        if (!string.IsNullOrEmpty(request.Term))
        {
            courses = courses.Where(x => x.Title.Contains(request.Term) || x.Description.Contains(request.Term));
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

        // filter by filter date
        if (request.SortBy == SortBy.Ascending)
        {
            courses = courses.OrderBy(x => x.Created);
        }
        else if (request.SortBy == SortBy.Descending)
        {
            courses = courses.OrderByDescending(x => x.Created);
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
        var mappedCourses = _mapper.Map<List<GetBriefCourseResponseModel>>(courses);
        var createPaginatedListResult = Pagination<GetBriefCourseResponseModel>.Create(mappedCourses.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

        if(createPaginatedListResult == null)
        {
            return new BaseResponse<Pagination<GetBriefCourseResponseModel>>
            {
                Success = false,
                Message = "Get paginated list course failed",
            };
        }

        return new BaseResponse<Pagination<GetBriefCourseResponseModel>>
        {
            Success = true,
            Message = "Get paginated list course successful",
            Data = createPaginatedListResult,
        };
    }
}
