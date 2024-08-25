using Application.Common;
using Application.Processions;
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
    public sealed record UpdateProcessionCommand : IRequest<BaseResponse<GetBriefProcessionResponseModel>>
    {
        [Required]
        public Guid Id { get; set; }
        public Guid? ParticipantId { get; set; }
        public Guid? TopicId { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class UpdateProcessionCommandHanler : IRequestHandler<UpdateProcessionCommand, BaseResponse<GetBriefProcessionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProcessionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefProcessionResponseModel>> Handle(UpdateProcessionCommand request, CancellationToken cancellationToken)
        {
            if (request.ParticipantId != null)
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
            }
            if (request.TopicId != null)
            {
                var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == request.TopicId);
                if (topic == null)
                {
                    return new BaseResponse<GetBriefProcessionResponseModel>
                    {
                        Success = false,
                        Message = "Topic not found",
                    };
                }
            }

            var procession = await _context.Processions.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (procession == null)
            {
                return new BaseResponse<GetBriefProcessionResponseModel>
                {
                    Success = false,
                    Message = "Procession is not found",
                    Errors = ["Procession is not found"]
                };
            }

            //_mapper.Map(request, participant);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = procession.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(procession, requestValue);
                    }
                }
            }

            var updateProcessionResult = _context.Update(procession);

            if (updateProcessionResult.Entity == null)
            {
                return new BaseResponse<GetBriefProcessionResponseModel>
                {
                    Success = false,
                    Message = "Update procession failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedProcessionResult = _mapper.Map<GetBriefProcessionResponseModel>(updateProcessionResult.Entity);

            return new BaseResponse<GetBriefProcessionResponseModel>
            {
                Success = true,
                Message = "Update procession successful",
                Data = mappedProcessionResult
            };
        }
    }
}
