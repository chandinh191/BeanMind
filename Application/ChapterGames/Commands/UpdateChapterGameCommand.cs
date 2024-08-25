using Application.ChapterGames;
using Application.Common;
using AutoMapper;
using Domain.Entities;
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
    public sealed record UpdateChapterGameCommand : IRequest<BaseResponse<GetBriefChapterGameResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public Guid? ChapterId { get; set; }
        public Guid? GameId { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class UpdateChapterGameCommandHanler : IRequestHandler<UpdateChapterGameCommand, BaseResponse<GetBriefChapterGameResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateChapterGameCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefChapterGameResponseModel>> Handle(UpdateChapterGameCommand request, CancellationToken cancellationToken)
        {
            if (request.ChapterId != null)
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
            }
            if (request.GameId != null)
            {
                var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == request.GameId);
                if (game == null)
                {
                    return new BaseResponse<GetBriefChapterGameResponseModel>
                    {
                        Success = false,
                        Message = "Game not found",
                    };
                }
            }
            var chapterGame = await _context.ChapterGames.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (chapterGame == null)
            {
                return new BaseResponse<GetBriefChapterGameResponseModel>
                {
                    Success = false,
                    Message = "Chapter game is not found",
                    Errors = ["Chapter game is not found"]
                };
            }

            //_mapper.Map(request, chapterGame);

            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = chapterGame.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(chapterGame, requestValue);
                    }
                }
            }

            var updateChapterResult = _context.Update(chapterGame);

            if (updateChapterResult.Entity == null)
            {
                return new BaseResponse<GetBriefChapterGameResponseModel>
                {
                    Success = false,
                    Message = "Update chapter failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedChapterResult = _mapper.Map<GetBriefChapterGameResponseModel>(updateChapterResult.Entity);

            return new BaseResponse<GetBriefChapterGameResponseModel>
            {
                Success = true,
                Message = "Update chapter successful",
                Data = mappedChapterResult
            };
        }
    }

}
