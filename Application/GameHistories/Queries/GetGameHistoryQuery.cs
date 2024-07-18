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

namespace Application.GameHistories.Queries
{
    public sealed record GetGameHistoryQuery : IRequest<BaseResponse<GetGameHistoryResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetGameHistoryQueryHanler : IRequestHandler<GetGameHistoryQuery, BaseResponse<GetGameHistoryResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetGameHistoryQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetGameHistoryResponseModel>> Handle(GetGameHistoryQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetGameHistoryResponseModel>
                {
                    Success = false,
                    Message = "Get game history failed",
                    Errors = ["Id required"],
                };
            }

            var gameHistory = await _context.GameHistories
                .Include(x => x.ApplicationUser)
                .Include(x => x.Game)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedGameHistory = _mapper.Map<GetGameHistoryResponseModel>(gameHistory);

            return new BaseResponse<GetGameHistoryResponseModel>
            {
                Success = true,
                Message = "Get game history successful",
                Data = mappedGameHistory
            };
        }
    }
}
