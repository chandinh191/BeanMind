using Application.ChapterGames;
using Application.Common;
using Application.Teachables;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChapterGames.Commands
{
    public sealed record DeleteChapterGameCommand : IRequest<BaseResponse<string>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteChapterGameCommandHanler : IRequestHandler<DeleteChapterGameCommand, BaseResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteChapterGameCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<string>> Handle(DeleteChapterGameCommand request, CancellationToken cancellationToken)
        {
            var chapterGame = await _context.ChapterGames.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (chapterGame == null)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Chapter game not found",
                };
            }
             _context.Remove(chapterGame);

            await _context.SaveChangesAsync(cancellationToken);

            return new BaseResponse<string>
            {
                Success = true,
                Message = "Delete chapter game successful",
            };
        }
    }
}
