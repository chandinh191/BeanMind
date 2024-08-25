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

namespace Application.WorksheetAttemptAnswers.Commands
{
    [AutoMap(typeof(Domain.Entities.WorksheetAttemptAnswer), ReverseMap = true)]
    public sealed record UpdateWorksheetAttemptAnswerCommand : IRequest<BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public Guid? WorksheetAttemptId { get; set; }
        public Guid? QuestionAnswerId { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class UpdateWorksheetAttemptAnswerCommandHanler : IRequestHandler<UpdateWorksheetAttemptAnswerCommand, BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateWorksheetAttemptAnswerCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>> Handle(UpdateWorksheetAttemptAnswerCommand request, CancellationToken cancellationToken)
        {
            if (request.WorksheetAttemptId != null)
            {
                var worksheetAttempt = await _context.WorksheetAttempts.FirstOrDefaultAsync(x => x.Id == request.WorksheetAttemptId);

                if (worksheetAttempt == null)
                {
                    return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
                    {
                        Success = false,
                        Message = "Worksheet attempt not found",
                    };
                }
            }
            if (request.QuestionAnswerId != null)
            {
                var questionAnswer = await _context.QuestionAnswers.FirstOrDefaultAsync(x => x.Id == request.QuestionAnswerId);

                if (questionAnswer == null)
                {
                    return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
                    {
                        Success = false,
                        Message = "Question answer not found",
                    };
                }
            }

            var worksheetAttemptAnswer = await _context.WorksheetAttemptAnswers.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (worksheetAttemptAnswer == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
                {
                    Success = false,
                    Message = "Worksheet attempt answer is not found",
                    Errors = ["Worksheet attempt answer is not found"]
                };
            }

            //_mapper.Map(request, topic);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = worksheetAttemptAnswer.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(worksheetAttemptAnswer, requestValue);
                    }
                }
            }

            var updateWorksheetAttemptAnswersResult = _context.Update(worksheetAttemptAnswer);

            if (updateWorksheetAttemptAnswersResult.Entity == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
                {
                    Success = false,
                    Message = "Update worksheet attempt answer failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedWorksheetAttemptAnswerResult = _mapper.Map<GetBriefWorksheetAttemptAnswerResponseModel>(updateWorksheetAttemptAnswersResult.Entity);

            return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
            {
                Success = true,
                Message = "Update worksheet attempt answer successful",
                Data = mappedWorksheetAttemptAnswerResult
            };
        }
    }
}
