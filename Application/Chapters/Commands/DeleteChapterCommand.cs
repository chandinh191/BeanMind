using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Chapters.Commands;

public sealed record DeleteChapterCommand : IRequest<BaseResponse<GetBriefChapterResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteChapterCommandHanler : IRequestHandler<DeleteChapterCommand, BaseResponse<GetBriefChapterResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteChapterCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefChapterResponseModel>> Handle(DeleteChapterCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _context.Chapters.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(chapter == null)
        {
            return new BaseResponse<GetBriefChapterResponseModel>
            {
                Success = false,
                Message = "Chapter not found",
            };
        }
        chapter.IsDeleted = true;


        var updateChapterResult = _context.Update(chapter);

        if (updateChapterResult.Entity == null)
        {
            return new BaseResponse<GetBriefChapterResponseModel>
            {
                Success = false,
                Message = "Delete chapter failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedChapterResult = _mapper.Map<GetBriefChapterResponseModel>(updateChapterResult.Entity);

        return new BaseResponse<GetBriefChapterResponseModel>
        {
            Success = true,
            Message = "Delete chapter successful",
            Data = mappedChapterResult
        };
    }
}
