using Application.Participants;
using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Participants.Commands
{
    public sealed record DeleteParticipantCommand : IRequest<BaseResponse<GetParticipantResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteParticipantCommandHanler : IRequestHandler<DeleteParticipantCommand, BaseResponse<GetParticipantResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteParticipantCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetParticipantResponseModel>> Handle(DeleteParticipantCommand request, CancellationToken cancellationToken)
        {
            var participant = await _context.Participant.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (participant == null)
            {
                return new BaseResponse<GetParticipantResponseModel>
                {
                    Success = false,
                    Message = "Participant not found",
                };
            }
            participant.IsDeleted = true;


            var updateParticipantResult = _context.Update(participant);

            if (updateParticipantResult.Entity == null)
            {
                return new BaseResponse<GetParticipantResponseModel>
                {
                    Success = false,
                    Message = "Delete participant failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedParticipantResult = _mapper.Map<GetParticipantResponseModel>(updateParticipantResult.Entity);

            return new BaseResponse<GetParticipantResponseModel>
            {
                Success = true,
                Message = "Delete participant successful",
                Data = mappedParticipantResult
            };
        }
    }

}
