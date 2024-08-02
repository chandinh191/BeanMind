using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Chapters.Commands;

[AutoMap(typeof(Domain.Entities.Chapter), ReverseMap = true)]
public sealed record CreateChapterCommand : IRequest<BaseResponse<GetBriefChapterResponseModel>>
{
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long and at most 40 characters")]
    // //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string Title { get; init; }
    [Required]
    public string Description { get; init; }
    [Required]
    public int Order { get; init; }
    [Required]
    public Guid CourseId { get; set; }
}

public class CreateChapterCommandHanler : IRequestHandler<CreateChapterCommand, BaseResponse<GetBriefChapterResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateChapterCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefChapterResponseModel>> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);

        if (course == null)
        {
            return new BaseResponse<GetBriefChapterResponseModel>
            {
                Success = false,
                Message = "Course not found",
            };
        }

        var chapter = _mapper.Map<Domain.Entities.Chapter>(request);
        var createChapterResult = await _context.AddAsync(chapter, cancellationToken);

        if(createChapterResult.Entity == null)
        {
            return new BaseResponse<GetBriefChapterResponseModel>
            {
                Success = false,
                Message = "Create chapter failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedChapterResult = _mapper.Map<GetBriefChapterResponseModel>(createChapterResult.Entity);

        return new BaseResponse<GetBriefChapterResponseModel>
        {
            Success = true,
            Message = "Create chapter successful",
            Data = mappedChapterResult
        };
    }
}
