using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Commands;

[AutoMap(typeof(Domain.Entities.Course), ReverseMap = true)]
public sealed record CreateCourseCommand : IRequest<BaseResponse<GetCourseResponseModel>>
{
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    // //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string Title { get; init; }
    [Required]
    public string Description { get; init; }
    [Required]
    public Guid SubjectId { get; set; }
}

public class CreateCourseCommandHanler : IRequestHandler<CreateCourseCommand, BaseResponse<GetCourseResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCourseCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetCourseResponseModel>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.SubjectId);

        if (subject == null)
        {
            return new BaseResponse<GetCourseResponseModel>
            {
                Success = false,
                Message = "Subject not found",
            };
        }

        var course = _mapper.Map<Domain.Entities.Course>(request);
        var createCourseResult = await _context.AddAsync(course, cancellationToken);

        if (createCourseResult.Entity == null)
        {
            return new BaseResponse<GetCourseResponseModel>
            {
                Success = false,
                Message = "Create course failed",
            };
        }

        var state = _context.Entry(course).State;

        await _context.SaveChangesAsync(cancellationToken);

        var mappedCourseResult = _mapper.Map<GetCourseResponseModel>(createCourseResult.Entity);

        return new BaseResponse<GetCourseResponseModel>
        {
            Success = true,
            Message = "Create course successful",
            Data = mappedCourseResult
        };
    }
}
