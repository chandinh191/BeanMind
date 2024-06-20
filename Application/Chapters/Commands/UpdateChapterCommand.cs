using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.Chapters.Commands;

[AutoMap(typeof(Domain.Entities.Chapter), ReverseMap = true)]
public sealed record UpdateChapterCommand : IRequest<BaseResponse<GetChapterResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    // //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string? Title { get; init; }
    [Required]
    public string? Description { get; init; }
    [Required]
    public Guid CourseId { get; set; }
}

public class UpdateChapterCommandHanler : IRequestHandler<UpdateChapterCommand, BaseResponse<GetChapterResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateChapterCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetChapterResponseModel>> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Course.FirstOrDefaultAsync(x => x.Id == request.CourseId);

        if (course == null)
        {
            return new BaseResponse<GetChapterResponseModel>
            {
                Success = false,
                Message = "Course not found",
            };
        }

        var chapter = await _context.Chapter.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(chapter == null)
        {
            return new BaseResponse<GetChapterResponseModel>
            {
                Success = false,
                Message = "Chapter is not found",
                Errors = ["Chapter is not found"]
            };
        }

        _mapper.Map(request, chapter);

        var updateChapterResult = _context.Update(chapter);

        if (updateChapterResult.Entity == null)
        {
            return new BaseResponse<GetChapterResponseModel>
            {
                Success = false,
                Message = "Update chapter failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedChapterResult = _mapper.Map<GetChapterResponseModel>(updateChapterResult.Entity);

        return new BaseResponse<GetChapterResponseModel>
        {
            Success = true,
            Message = "Update chapter successful",
            Data = mappedChapterResult
        };
    }
}
