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
using Application.Topics;

namespace Application.Participants.Queries
{
    public sealed record GetParticipantQuery : IRequest<BaseResponse<GetParticipantResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetParticipantQueryHanler : IRequestHandler<GetParticipantQuery, BaseResponse<GetParticipantResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetParticipantQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetParticipantResponseModel>> Handle(GetParticipantQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetParticipantResponseModel>
                {
                    Success = false,
                    Message = "Get participant failed",
                    Errors = ["Id required"],
                };
            }

            var participant = await _context.Participants
                .Include(o => o.Session)
                .Include(o => o.Enrollment)
                .Include(o => o.Processions) .ThenInclude(o => o.Topic)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedParticipant = _mapper.Map<GetParticipantResponseModel>(participant);

            mappedParticipant.LearnedTopics = [];

            var learnedTopics = new List<Domain.Entities.Topic>();

            var enrollmentData = await _context.Enrollments
                .Include(x => x.Participants)
                .ThenInclude(x => x.Processions)
                .ThenInclude(x => x.Topic)
                .FirstOrDefaultAsync(x => x.Id.Equals(participant.EnrollmentId), cancellationToken);

            foreach (var p in enrollmentData.Participants)
            {
                foreach (var t in p.Processions)
                {
                    learnedTopics.Add(t.Topic);
                }
            }
            mappedParticipant.LearnedTopics = learnedTopics
                .Select(x => _mapper.Map<GetBriefTopicResponseModel>(x)).ToList();

            return new BaseResponse<GetParticipantResponseModel>
            {
                Success = true,
                Message = "Get participant successful",
                Data = mappedParticipant
            };
        }
    }
}
