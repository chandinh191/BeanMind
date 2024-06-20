using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Chapters.Commands;

public sealed record DeleteChapterCommand : IRequest<BaseResponse<GetChapterResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteChapterCommandHanler : IRequestHandler<DeleteChapterCommand, BaseResponse<GetChapterResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteChapterCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetChapterResponseModel>> Handle(DeleteChapterCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _context.Chapter.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(chapter == null)
        {
            return new BaseResponse<GetChapterResponseModel>
            {
                Success = false,
                Message = "Chapter not found",
            };
        }
        chapter.IsDeleted = true;


        var updateChapterResult = _context.Update(chapter);

        if (updateChapterResult.Entity == null)
        {
            return new BaseResponse<GetChapterResponseModel>
            {
                Success = false,
                Message = "Delete chapter failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedChapterResult = _mapper.Map<GetChapterResponseModel>(updateChapterResult.Entity);

        return new BaseResponse<GetChapterResponseModel>
        {
            Success = true,
            Message = "Delete chapter successful",
            Data = mappedChapterResult
        };
    }
}
