using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Courses.Queries;

public sealed record GetCourseQuery : IRequest<BaseResponse<GetCourseResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetCourseQueryHanler : IRequestHandler<GetCourseQuery, BaseResponse<GetCourseResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCourseQueryHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetCourseResponseModel>> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            return new BaseResponse<GetCourseResponseModel>
            {
                Success = false,
                Message = "Get course failed",
                Errors = ["Id required"],
            };
        }

        var course = await _context.Courses
            .Include(x => x.Subject)
            .Include(x => x.Chapters)
            //.Include(x => x.StudentInCourses)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

        var mappedCourse = _mapper.Map<GetCourseResponseModel>(course);

        return new BaseResponse<GetCourseResponseModel>
        {
            Success = true,
            Message = "Get course successful",
            Data = mappedCourse
        };
    }
}
