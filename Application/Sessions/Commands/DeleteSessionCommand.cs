using Application.Common;
using Application.Questions;
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

namespace Application.Sessions.Commands
{
    public sealed record DeleteSessionCommand : IRequest<BaseResponse<GetBriefSessionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteSessionCommandHanler : IRequestHandler<DeleteSessionCommand, BaseResponse<GetBriefSessionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteSessionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefSessionResponseModel>> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (session == null)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "Session not found",
                };
            }

            session.IsDeleted = true;

            var updateSessionResult = _context.Update(session);

            if (updateSessionResult.Entity == null)
            {
                return new BaseResponse<GetBriefSessionResponseModel>
                {
                    Success = false,
                    Message = "Delete session failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedSessionResult = _mapper.Map<GetBriefSessionResponseModel>(updateSessionResult.Entity);

            return new BaseResponse<GetBriefSessionResponseModel>
            {
                Success = true,
                Message = "Delete session successful",
                Data = mappedSessionResult
            };
        }
    }
}
