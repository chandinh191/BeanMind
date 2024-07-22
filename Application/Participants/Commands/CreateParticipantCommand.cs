using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Enrollments;
using Application.Sessions;
using System.ComponentModel.DataAnnotations;

namespace Application.Participants.Commands
{
    [AutoMap(typeof(Domain.Entities.Participant), ReverseMap = true)]
    public sealed record CreateParticipantCommand : IRequest<BaseResponse<GetBriefParticipantResponseModel>>
    {
        [Required]
        public Guid EnrollmentId { get; set; }
        [Required]
        public Guid SessionId { get; set; }
        public bool IsPresent { get; set; } = true;
    }

    public class CreateParticipantCommandHanler : IRequestHandler<CreateParticipantCommand, BaseResponse<GetBriefParticipantResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateParticipantCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefParticipantResponseModel>> Handle(CreateParticipantCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(x => x.Id == request.EnrollmentId);

            if (enrollment == null)
            {
                return new BaseResponse<GetBriefParticipantResponseModel>
                {
                    Success = false,
                    Message = "Enrollment not found",
                };
            }
            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.Id == request.SessionId);

            if (session == null)
            {
                return new BaseResponse<GetBriefParticipantResponseModel>
                {
                    Success = false,
                    Message = "Session not found",
                };
            }

            var participant = _mapper.Map<Domain.Entities.Participant>(request);
            var createParticipantResult = await _context.AddAsync(participant, cancellationToken);

            if (createParticipantResult.Entity == null)
            {
                return new BaseResponse<GetBriefParticipantResponseModel>
                {
                    Success = false,
                    Message = "Create participant failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedParticipantResult = _mapper.Map<GetBriefParticipantResponseModel>(createParticipantResult.Entity);

            return new BaseResponse<GetBriefParticipantResponseModel>
            {
                Success = true,
                Message = "Create participant successful",
                Data = mappedParticipantResult
            };
        }
    }
}
