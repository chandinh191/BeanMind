using Application.Common;
using Application.Games;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Games.Commands
{
    public class CreateChapterGameModel
    {
        [Required]
        public Guid ChapterId { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Game), ReverseMap = true)]
    public sealed record CreateGameCommand : IRequest<BaseResponse<GetBriefGameResponseModel>>
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ItemStoreJson { get; set; }
        public string AnimalJson { get; set; }
        public List<CreateChapterGameModel> ChapterGames { get; set; }
    }

    public class CreateGameCommandHanler : IRequestHandler<CreateGameCommand, BaseResponse<GetBriefGameResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateGameCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefGameResponseModel>> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            //var game = _mapper.Map<Domain.Entities.Game>(request);
            var game = new Game()
            {
                Name = request.Name,
                Description = request.Description,
                Image = request.Image,
                ItemStoreJson = request.ItemStoreJson,
                AnimalJson = request.AnimalJson,
            };

            var createGameResult = await _context.AddAsync(game, cancellationToken);

            if (createGameResult.Entity == null)
            {
                return new BaseResponse<GetBriefGameResponseModel>
                {
                    Success = false,
                    Message = "Create game failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);


            if (request.ChapterGames != null && request.ChapterGames.Count > 0)
            {
                foreach (var chapter in request.ChapterGames)
                {
                    var chapterGameModel = new ChapterGame
                    {
                        ChapterId = chapter.ChapterId,
                        GameId = game.Id,
                    };
                    var createChapterGameResult = await _context.AddAsync(chapterGameModel, cancellationToken);
                }
                await _context.SaveChangesAsync(cancellationToken);
            }


            var mappedGameResult = _mapper.Map<GetBriefGameResponseModel>(createGameResult.Entity);

            return new BaseResponse<GetBriefGameResponseModel>
            {
                Success = true,
                Message = "Create game successful",
                Data = mappedGameResult
            };
        }
    }
}
