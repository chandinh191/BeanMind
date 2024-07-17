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
    public sealed record CreateGameCommand : IRequest<BaseResponse<GetBriefGameResponseModel>>
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
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
            var game = _mapper.Map<Domain.Entities.Game>(request);
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
