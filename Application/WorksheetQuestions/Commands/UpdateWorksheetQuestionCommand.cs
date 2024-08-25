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

namespace Application.WorksheetQuestions.Commands
{
    [AutoMap(typeof(Domain.Entities.WorksheetQuestion), ReverseMap = true)]
    public sealed record UpdateWorksheetQuestionCommand : IRequest<BaseResponse<GetBriefWorksheetQuestionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public Guid? QuestionId { get; set; }
        public Guid? WorksheetId { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class UpdateWorksheetQuestionCommandHanler : IRequestHandler<UpdateWorksheetQuestionCommand, BaseResponse<GetBriefWorksheetQuestionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateWorksheetQuestionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefWorksheetQuestionResponseModel>> Handle(UpdateWorksheetQuestionCommand request, CancellationToken cancellationToken)
        {
            if (request.QuestionId != null)
            {
                var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);

                if (question == null)
                {
                    return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
                    {
                        Success = false,
                        Message = "Question not found",
                    };
                }
            }
            if (request.WorksheetId != null)
            {
                var worksheet = await _context.Worksheets.FirstOrDefaultAsync(x => x.Id == request.WorksheetId);

                if (worksheet == null)
                {
                    return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
                    {
                        Success = false,
                        Message = "Worksheet not found",
                    };
                }
            }

            var worksheetQuestion = await _context.WorksheetQuestions.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (worksheetQuestion == null)
            {
                return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
                {
                    Success = false,
                    Message = "Worksheet question is not found",
                    Errors = ["Worksheet question is not found"]
                };
            }

            //_mapper.Map(request, topic);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = worksheetQuestion.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(worksheetQuestion, requestValue);
                    }
                }
            }

            var updateWorksheetQuestionResult = _context.Update(worksheetQuestion);

            if (updateWorksheetQuestionResult.Entity == null)
            {
                return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
                {
                    Success = false,
                    Message = "Update worksheet question failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedWorksheetQuestionResult = _mapper.Map<GetBriefWorksheetQuestionResponseModel>(updateWorksheetQuestionResult.Entity);

            return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
            {
                Success = true,
                Message = "Update worksheet question successful",
                Data = mappedWorksheetQuestionResult
            };
        }
    }
}
