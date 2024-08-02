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

namespace Application.Processions.Commands
{
    [AutoMap(typeof(Domain.Entities.Procession), ReverseMap = true)]
    public sealed record CreateProcessionCommand : IRequest<BaseResponse<GetBriefProcessionResponseModel>>
    {
        [Required]
        public Guid ParticipantId { get; set; }
        [Required]
        public Guid TopicId { get; set; }
    }

    public class CreateProcessionCommandHanler : IRequestHandler<CreateProcessionCommand, BaseResponse<GetBriefProcessionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateProcessionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefProcessionResponseModel>> Handle(CreateProcessionCommand request, CancellationToken cancellationToken)
        {
            var participant = await _context.Participants.FirstOrDefaultAsync(x => x.Id == request.ParticipantId);
            if (participant == null)
            {
                return new BaseResponse<GetBriefProcessionResponseModel>
                {
                    Success = false,
                    Message = "Participant not found",
                };
            }

            var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == request.TopicId);
            if (topic == null)
            {
                return new BaseResponse<GetBriefProcessionResponseModel>
                {
                    Success = false,
                    Message = "Topic not found",
                };
            }

            var procession = _mapper.Map<Domain.Entities.Procession>(request);
            var createProcessionResult = await _context.AddAsync(procession, cancellationToken);

            if (createProcessionResult.Entity == null)
            {
                return new BaseResponse<GetBriefProcessionResponseModel>
                {
                    Success = false,
                    Message = "Create procession failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedProcessionResult = _mapper.Map<GetBriefProcessionResponseModel>(createProcessionResult.Entity);

            return new BaseResponse<GetBriefProcessionResponseModel>
            {
                Success = true,
                Message = "Create procession successful",
                Data = mappedProcessionResult
            };
        }
    }
}
