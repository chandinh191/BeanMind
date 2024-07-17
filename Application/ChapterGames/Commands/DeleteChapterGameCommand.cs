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
    public sealed record DeleteChapterGameCommand : IRequest<BaseResponse<GetBriefChapterGameResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteChapterGameCommandHanler : IRequestHandler<DeleteChapterGameCommand, BaseResponse<GetBriefChapterGameResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteChapterGameCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefChapterGameResponseModel>> Handle(DeleteChapterGameCommand request, CancellationToken cancellationToken)
        {
            var chapterGame = await _context.Chapters.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (chapterGame == null)
            {
                return new BaseResponse<GetBriefChapterGameResponseModel>
                {
                    Success = false,
                    Message = "Chapter not found",
                };
            }
            chapterGame.IsDeleted = true;


            var updateChapterGameResult = _context.Update(chapterGame);

            if (updateChapterGameResult.Entity == null)
            {
                return new BaseResponse<GetBriefChapterGameResponseModel>
                {
                    Success = false,
                    Message = "Delete chapter game failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedChapterGameResult = _mapper.Map<GetBriefChapterGameResponseModel>(updateChapterGameResult.Entity);

            return new BaseResponse<GetBriefChapterGameResponseModel>
            {
                Success = true,
                Message = "Delete chapter game successful",
                Data = mappedChapterGameResult
            };
        }
    }
}
