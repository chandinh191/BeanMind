using Application.ChapterGames;
using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChapterGames.Commands
{
    [AutoMap(typeof(Domain.Entities.ChapterGame), ReverseMap = true)]
    public sealed record CreateChapterGameCommand : IRequest<BaseResponse<GetBriefChapterGameResponseModel>>
    {
        [Required]
        public Guid ChapterId { get; set; }
        [Required]
        public Guid GameId { get; set; }
    }

    public class CreateChapterGameCommandHanler : IRequestHandler<CreateChapterGameCommand, BaseResponse<GetBriefChapterGameResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateChapterGameCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefChapterGameResponseModel>> Handle(CreateChapterGameCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _context.Chapters.FirstOrDefaultAsync(x => x.Id == request.ChapterId);
            if (chapter == null)
            {
                return new BaseResponse<GetBriefChapterGameResponseModel>
                {
                    Success = false,
                    Message = "Chapter not found",
                };
            }
            var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == request.GameId);
            if (game == null)
            {
                return new BaseResponse<GetBriefChapterGameResponseModel>
                {
                    Success = false,
                    Message = "Game not found",
                };
            }

            var chapterGame = _mapper.Map<Domain.Entities.ChapterGame>(request);
            var createChapterGameResult = await _context.AddAsync(chapterGame, cancellationToken);

            if (createChapterGameResult.Entity == null)
            {
                return new BaseResponse<GetBriefChapterGameResponseModel>
                {
                    Success = false,
                    Message = "Create chapter game failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedChapterGameResult = _mapper.Map<GetBriefChapterGameResponseModel>(createChapterGameResult.Entity);

            return new BaseResponse<GetBriefChapterGameResponseModel>
            {
                Success = true,
                Message = "Create chapter game successful",
                Data = mappedChapterGameResult
            };
        }
    }

}
