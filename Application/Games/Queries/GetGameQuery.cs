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

namespace Application.Games.Queries
{
    public sealed record GetGameQuery : IRequest<BaseResponse<GetGameResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetGameQueryHanler : IRequestHandler<GetGameQuery, BaseResponse<GetGameResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetGameQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetGameResponseModel>> Handle(GetGameQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetGameResponseModel>
                {
                    Success = false,
                    Message = "Get game failed",
                    Errors = ["Id required"],
                };
            }

            var game = await _context.Games
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedGame = _mapper.Map<GetGameResponseModel>(game);

            return new BaseResponse<GetGameResponseModel>
            {
                Success = true,
                Message = "Get game successful",
                Data = mappedGame
            };
        }
    }
}
