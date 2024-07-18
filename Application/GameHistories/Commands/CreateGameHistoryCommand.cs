using Application.Common;
using Application.GameHistories;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GameHistories.Commands
{
    [AutoMap(typeof(Domain.Entities.GameHistory), ReverseMap = true)]
    public sealed record CreateGameHistoryCommand : IRequest<BaseResponse<GetBriefGameHistoryResponseModel>>
    {
        [Required]
        public Guid GameId { get; set; }
        [Required]
        public string? ApplicationUserId { get; set; }
        [Required]
        public int Point { get; set; }
    }

    public class CreateGameHistoryCommandHanler : IRequestHandler<CreateGameHistoryCommand, BaseResponse<GetBriefGameHistoryResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateGameHistoryCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefGameHistoryResponseModel>> Handle(CreateGameHistoryCommand request, CancellationToken cancellationToken)
        {
            var gameHistory = _mapper.Map<Domain.Entities.GameHistory>(request);
            var createGameHistoryResult = await _context.AddAsync(gameHistory, cancellationToken);

            if (createGameHistoryResult.Entity == null)
            {
                return new BaseResponse<GetBriefGameHistoryResponseModel>
                {
                    Success = false,
                    Message = "Create game history failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedGameHistoryResult = _mapper.Map<GetBriefGameHistoryResponseModel>(createGameHistoryResult.Entity);

            return new BaseResponse<GetBriefGameHistoryResponseModel>
            {
                Success = true,
                Message = "Create game history successful",
                Data = mappedGameHistoryResult
            };
        }
    }
}
