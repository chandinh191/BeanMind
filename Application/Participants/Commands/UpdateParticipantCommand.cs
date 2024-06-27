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
using Application.Enrollments;
using Application.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Application.Participants.Commands
{
    [AutoMap(typeof(Domain.Entities.Participant), ReverseMap = true)]
    public sealed record UpdateParticipantCommand : IRequest<BaseResponse<GetParticipantResponseModel>>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid EnrollmentId { get; set; }
        [Required]
        public Guid SessionId { get; set; }
        public bool IsPresent { get; set; }
    }

    public class UpdateParticipantCommandHanler : IRequestHandler<UpdateParticipantCommand, BaseResponse<GetParticipantResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateParticipantCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetParticipantResponseModel>> Handle(UpdateParticipantCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _context.Enrollment.FirstOrDefaultAsync(x => x.Id == request.EnrollmentId);

            if (enrollment == null)
            {
                return new BaseResponse<GetParticipantResponseModel>
                {
                    Success = false,
                    Message = "Enrollment not found",
                };
            }
            var session = await _context.Session.FirstOrDefaultAsync(x => x.Id == request.SessionId);

            if (session == null)
            {
                return new BaseResponse<GetParticipantResponseModel>
                {
                    Success = false,
                    Message = "Session not found",
                };
            }

            var participant = await _context.Participant.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (participant == null)
            {
                return new BaseResponse<GetParticipantResponseModel>
                {
                    Success = false,
                    Message = "Participant is not found",
                    Errors = ["Participant is not found"]
                };
            }

            _mapper.Map(request, participant);

            var updateParticipantResult = _context.Update(participant);

            if (updateParticipantResult.Entity == null)
            {
                return new BaseResponse<GetParticipantResponseModel>
                {
                    Success = false,
                    Message = "Update participant failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedParticipantResult = _mapper.Map<GetParticipantResponseModel>(updateParticipantResult.Entity);

            return new BaseResponse<GetParticipantResponseModel>
            {
                Success = true,
                Message = "Update participant successful",
                Data = mappedParticipantResult
            };
        }
    }
}
