using Application.Common;
using Application.GameHistories;
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

namespace Application.GameHistories.Commands
{
    public sealed record DeleteGameHistoryCommand : IRequest<BaseResponse<GetBriefGameHistoryResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteGameHistoryCommandHanler : IRequestHandler<DeleteGameHistoryCommand, BaseResponse<GetBriefGameHistoryResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteGameHistoryCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefGameHistoryResponseModel>> Handle(DeleteGameHistoryCommand request, CancellationToken cancellationToken)
        {
            var gameHistory = await _context.GameHistories.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (gameHistory == null)
            {
                return new BaseResponse<GetBriefGameHistoryResponseModel>
                {
                    Success = false,
                    Message = "Game history not found",
                };
            }
            gameHistory.IsDeleted = true;

            var updateGameHistoryResult = _context.Update(gameHistory);

            if (updateGameHistoryResult.Entity == null)
            {
                return new BaseResponse<GetBriefGameHistoryResponseModel>
                {
                    Success = false,
                    Message = "Delete game history failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedGameHistoryResult = _mapper.Map<GetBriefGameHistoryResponseModel>(updateGameHistoryResult.Entity);

            return new BaseResponse<GetBriefGameHistoryResponseModel>
            {
                Success = true,
                Message = "Delete game history successful",
                Data = mappedGameHistoryResult
            };
        }
    }
}
