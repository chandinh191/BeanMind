using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Chapters.Queries;

public sealed record GetChapterQuery : IRequest<BaseResponse<GetChapterResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetChapterQueryHanler : IRequestHandler<GetChapterQuery, BaseResponse<GetChapterResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetChapterQueryHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetChapterResponseModel>> Handle(GetChapterQuery request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            return new BaseResponse<GetChapterResponseModel>
            {
                Success = false,
                Message = "Get chapter failed",
                Errors = ["Id required"],
            };
        }

        var chapter = await _context.Chapters
            .Include(x => x.Course)
            .Include(x => x.Topics)
            .Include(x => x.ChapterGames)
            .Include(x => x.WorksheetTemplates)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

        var mappedChapter = _mapper.Map<GetChapterResponseModel>(chapter);

        return new BaseResponse<GetChapterResponseModel>
        {
            Success = true,
            Message = "Get chapter successful",
            Data = mappedChapter
        };
    }
}
