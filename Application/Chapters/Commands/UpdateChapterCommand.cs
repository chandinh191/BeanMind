using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.Chapters.Commands;

[AutoMap(typeof(Domain.Entities.Chapter), ReverseMap = true)]
public sealed record UpdateChapterCommand : IRequest<BaseResponse<GetBriefChapterResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    // //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string? Title { get; init; }
    public string? Description { get; init; }
    public Guid? CourseId { get; set; }
}

public class UpdateChapterCommandHanler : IRequestHandler<UpdateChapterCommand, BaseResponse<GetBriefChapterResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateChapterCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefChapterResponseModel>> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
    {
        if (request.CourseId != null)
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
        }
        var chapter = await _context.Chapters.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(chapter == null)
        {
            return new BaseResponse<GetBriefChapterResponseModel>
            {
                Success = false,
                Message = "Chapter is not found",
                Errors = ["Chapter is not found"]
            };
        }

        //_mapper.Map(request, chapter);

        // Use reflection to update non-null properties
        foreach (var property in request.GetType().GetProperties())
        {
            var requestValue = property.GetValue(request);
            if (requestValue != null)
            {
                var chapterProperty = chapter.GetType().GetProperty(property.Name);
                if (chapterProperty != null)
                {
                    chapterProperty.SetValue(chapter, requestValue);
                }
            }
        }

        var updateChapterResult = _context.Update(chapter);

        if (updateChapterResult.Entity == null)
        {
            return new BaseResponse<GetBriefChapterResponseModel>
            {
                Success = false,
                Message = "Update chapter failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedChapterResult = _mapper.Map<GetBriefChapterResponseModel>(updateChapterResult.Entity);

        return new BaseResponse<GetBriefChapterResponseModel>
        {
            Success = true,
            Message = "Update chapter successful",
            Data = mappedChapterResult
        };
    }
}
