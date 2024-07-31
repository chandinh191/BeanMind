using Application.Common;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GameHistories.Queries
{

    public sealed record GetGameHistoryLeaderBoardQuery : IRequest<BaseResponse<List<GetGameHistoryResponseModel>>>
    {
        [Required]
        public Guid GameId { get; set; }
        public int Top { get; set; } = 10;
        public SortBy SortBy { get; init; } = SortBy.Descending;
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetGameHistoryLeaderBoardQueryHandler : IRequestHandler<GetGameHistoryLeaderBoardQuery, BaseResponse<List<GetGameHistoryResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetGameHistoryLeaderBoardQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<GetGameHistoryResponseModel>>> Handle(GetGameHistoryLeaderBoardQuery request, CancellationToken cancellationToken)
        {
            var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == request.GameId);
            if (game == null)
            {
                return new BaseResponse<List<GetGameHistoryResponseModel>>
                {
                    Success = false,
                    Message = "Game not found",
                };
            }

            var gameHistories = _context.GameHistories
                .Include(o => o.ApplicationUser)
                .Include(o => o.Game)
                .AsQueryable();

            // filter by GameId
            if (request.GameId != Guid.Empty)
            {
                gameHistories = gameHistories.Where(x => x.GameId == request.GameId);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                gameHistories = gameHistories.Where(o => o.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                gameHistories = gameHistories.Where(o => o.Created <= request.EndTime);
            }

            // Filter duplicate users, selecting the one with the highest points
            List<GameHistory> topGameHistories;
            if (request.SortBy == SortBy.Ascending)
            {
                topGameHistories = gameHistories
                    .GroupBy(x => x.ApplicationUserId)
                    .Select(g => g.OrderBy(x => x.Point).First())
                    .Take(request.Top)
                    .ToList();
            }
            else
            {
                topGameHistories = gameHistories
                    .GroupBy(x => x.ApplicationUserId)
                    .Select(g => g.OrderByDescending(x => x.Point).First())
                    .Take(request.Top)
                    .ToList();
            }

            // convert the list of item to list of response model
            var mappedGameHistories = _mapper.Map<List<GetGameHistoryResponseModel>>(topGameHistories);

            if (mappedGameHistories == null)
            {
                return new BaseResponse<List<GetGameHistoryResponseModel>>
                {
                    Success = false,
                    Message = "Get game history leader board failed",
                };
            }

            return new BaseResponse<List<GetGameHistoryResponseModel>>
            {
                Success = true,
                Message = "Get game history leader board  successful",
                Data = mappedGameHistories,
            };
        }
    }
}
