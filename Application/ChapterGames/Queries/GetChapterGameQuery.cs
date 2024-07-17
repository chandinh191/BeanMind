using Application.ChapterGames;
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

namespace Application.ChapterGames.Queries
{
    public sealed record GetChapterGameQuery : IRequest<BaseResponse<GetChapterGameResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetChapterGameQueryHanler : IRequestHandler<GetChapterGameQuery, BaseResponse<GetChapterGameResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetChapterGameQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetChapterGameResponseModel>> Handle(GetChapterGameQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetChapterGameResponseModel>
                {
                    Success = false,
                    Message = "Get chapter game failed",
                    Errors = ["Id required"],
                };
            }

            var chapterGame = await _context.ChapterGames
                .Include(x => x.Chapter)
                .Include(x => x.Game)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedChapterGame = _mapper.Map<GetChapterGameResponseModel>(chapterGame);

            return new BaseResponse<GetChapterGameResponseModel>
            {
                Success = true,
                Message = "Get chapter game successful",
                Data = mappedChapterGame
            };
        }
    }

}
