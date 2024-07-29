using Application.Common;
using Application.Games;
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

namespace Application.Games.Commands
{
    [AutoMap(typeof(Domain.Entities.Game), ReverseMap = true)]
    public sealed record UpdateGameCommand : IRequest<BaseResponse<GetBriefGameResponseModel>>
    {
        [Required]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ItemStoreJson { get; set; }
        public string? AnimalJson { get; set; }
    }

    public class UpdateGameCommandHanler : IRequestHandler<UpdateGameCommand, BaseResponse<GetBriefGameResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateGameCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefGameResponseModel>> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {

            var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (game == null)
            {
                return new BaseResponse<GetBriefGameResponseModel>
                {
                    Success = false,
                    Message = "Game is not found",
                    Errors = ["Game is not found"]
                };
            }

            //_mapper.Map(request, enrollment);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = game.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(game, requestValue);
                    }
                }
            }

            var updateGameResult = _context.Update(game);

            if (updateGameResult.Entity == null)
            {
                return new BaseResponse<GetBriefGameResponseModel>
                {
                    Success = false,
                    Message = "Update game failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedGameResult = _mapper.Map<GetBriefGameResponseModel>(updateGameResult.Entity);

            return new BaseResponse<GetBriefGameResponseModel>
            {
                Success = true,
                Message = "Update game successful",
                Data = mappedGameResult
            };
        }
    }
}
