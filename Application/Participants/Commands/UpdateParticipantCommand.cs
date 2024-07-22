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
using Domain.Entities;

namespace Application.Participants.Commands
{
    [AutoMap(typeof(Domain.Entities.Participant), ReverseMap = true)]
    public sealed record UpdateParticipantCommand : IRequest<BaseResponse<GetBriefParticipantResponseModel>>
    {
        [Required]
        public Guid Id { get; set; }
        public Guid? EnrollmentId { get; set; }
        public Guid? SessionId { get; set; }
        public bool? IsPresent { get; set; }
    }

    public class UpdateParticipantCommandHanler : IRequestHandler<UpdateParticipantCommand, BaseResponse<GetBriefParticipantResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateParticipantCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefParticipantResponseModel>> Handle(UpdateParticipantCommand request, CancellationToken cancellationToken)
        {
            if (request.EnrollmentId != null)
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
            }
            if (request.SessionId != null)
            {
                var session = await _context.Sessions.FirstOrDefaultAsync(x => x.Id == request.SessionId);
                if (session == null)
                {
                    return new BaseResponse<GetBriefParticipantResponseModel>
                    {
                        Success = false,
                        Message = "Session not found",
                    };
                }
            }

            var participant = await _context.Participants.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (participant == null)
            {
                return new BaseResponse<GetBriefParticipantResponseModel>
                {
                    Success = false,
                    Message = "Participant is not found",
                    Errors = ["Participant is not found"]
                };
            }

            //_mapper.Map(request, participant);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var chapterProperty = participant.GetType().GetProperty(property.Name);
                    if (chapterProperty != null)
                    {
                        chapterProperty.SetValue(participant, requestValue);
                    }
                }
            }

            var updateParticipantResult = _context.Update(participant);

            if (updateParticipantResult.Entity == null)
            {
                return new BaseResponse<GetBriefParticipantResponseModel>
                {
                    Success = false,
                    Message = "Update participant failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedParticipantResult = _mapper.Map<GetBriefParticipantResponseModel>(updateParticipantResult.Entity);

            return new BaseResponse<GetBriefParticipantResponseModel>
            {
                Success = true,
                Message = "Update participant successful",
                Data = mappedParticipantResult
            };
        }
    }
}
