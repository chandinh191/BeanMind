using Application.Common;
using Application.Enrollments;
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
    [AutoMap(typeof(Domain.Entities.GameHistory), ReverseMap = true)]
    public sealed record UpdateGameHistoryCommand : IRequest<BaseResponse<GetBriefGameHistoryResponseModel>>
    {
        [Required]
        public Guid Id { get; set; }
        public Guid? GameId { get; set; }
        public string? ApplicationUserId { get; set; }
        public int? Point { get; set; }
    }

    public class UpdateGameHistoryCommandHanler : IRequestHandler<UpdateGameHistoryCommand, BaseResponse<GetBriefGameHistoryResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateGameHistoryCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefGameHistoryResponseModel>> Handle(UpdateGameHistoryCommand request, CancellationToken cancellationToken)
        {
            if (request.GameId != null)
            {
                var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == request.GameId);
                if (game == null)
                {
                    return new BaseResponse<GetBriefGameHistoryResponseModel>
                    {
                        Success = false,
                        Message = "Game not found",
                    };
                }
            }

            if (request.ApplicationUserId != null)
            {
                var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id.Equals(request.ApplicationUserId));
                if (applicationUser == null)
                {
                    return new BaseResponse<GetBriefGameHistoryResponseModel>
                    {
                        Success = false,
                        Message = "User not found",
                    };
                }
            }


            var gameHistory = await _context.GameHistories.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (gameHistory == null)
            {
                return new BaseResponse<GetBriefGameHistoryResponseModel>
                {
                    Success = false,
                    Message = "Game history is not found",
                    Errors = ["Game history  is not found"]
                };
            }

            //_mapper.Map(request, enrollment);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = gameHistory.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(gameHistory, requestValue);
                    }
                }
            }

            var updateGameHistoryResult = _context.Update(gameHistory);

            if (updateGameHistoryResult.Entity == null)
            {
                return new BaseResponse<GetBriefGameHistoryResponseModel>
                {
                    Success = false,
                    Message = "Update game history failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedGameHistoryResult = _mapper.Map<GetBriefGameHistoryResponseModel>(updateGameHistoryResult.Entity);

            return new BaseResponse<GetBriefGameHistoryResponseModel>
            {
                Success = true,
                Message = "Update game history successful",
                Data = mappedGameHistoryResult
            };
        }
    }
}
