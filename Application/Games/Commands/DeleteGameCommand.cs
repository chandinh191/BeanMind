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

namespace Application.Games.Commands
{
    public sealed record DeleteGameCommand : IRequest<BaseResponse<GetBriefGameResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteGameCommandHanler : IRequestHandler<DeleteGameCommand, BaseResponse<GetBriefGameResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteGameCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefGameResponseModel>> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
        {
            var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (game == null)
            {
                return new BaseResponse<GetBriefGameResponseModel>
                {
                    Success = false,
                    Message = "Game not found",
                };
            }
            game.IsDeleted = true;

            var updateGameResult = _context.Update(game);

            if (updateGameResult.Entity == null)
            {
                return new BaseResponse<GetBriefGameResponseModel>
                {
                    Success = false,
                    Message = "Delete game failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedGameResult = _mapper.Map<GetBriefGameResponseModel>(updateGameResult.Entity);

            return new BaseResponse<GetBriefGameResponseModel>
            {
                Success = true,
                Message = "Delete game successful",
                Data = mappedGameResult
            };
        }
    }
}
