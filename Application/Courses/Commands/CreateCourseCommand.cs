using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Commands;

[AutoMap(typeof(Domain.Entities.Course), ReverseMap = true)]
public sealed record CreateCourseCommand : IRequest<BaseResponse<GetBriefCourseResponseModel>>
{
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    // //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string Title { get; init; }
    [Required]
    public string Description { get; init; }
    [Required]
    public int Price { get; set; }
    public string? ImageURL { get; set; }
    [Required]
    public int TotalSlot { get; set; }
    [Required]
    public Guid SubjectId { get; set; }
    [Required]
    public Guid ProgramTypeId { get; set; }
    [Required]
    public Guid CourseLevelId { get; set; }
}

public class CreateCourseCommandHanler : IRequestHandler<CreateCourseCommand, BaseResponse<GetBriefCourseResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCourseCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefCourseResponseModel>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.SubjectId);
        if (subject == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Subject not found",
            };
        }
        var programType = await _context.ProgramTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ProgramTypeId);
        if (programType == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Program type not found",
            };
        }
        var courseLevel = await _context.CourseLevels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.CourseLevelId);
        if (courseLevel == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Course level not found",
            };
        }

        var course = _mapper.Map<Domain.Entities.Course>(request);
        var createCourseResult = await _context.AddAsync(course, cancellationToken);

        if (createCourseResult.Entity == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Create course failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedCourseResult = _mapper.Map<GetBriefCourseResponseModel>(createCourseResult.Entity);

        return new BaseResponse<GetBriefCourseResponseModel>
        {
            Success = true,
            Message = "Create course successful",
            Data = mappedCourseResult
        };
    }
}
