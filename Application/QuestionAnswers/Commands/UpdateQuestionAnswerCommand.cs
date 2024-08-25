using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.QuestionAnswers.Commands;

[AutoMap(typeof(Domain.Entities.QuestionAnswer), ReverseMap = true)]
public sealed record UpdateQuestionAnswerCommand : IRequest<BaseResponse<GetBriefQuestionAnswerResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    public Guid? QuestionId { get; set; }
    public string? Content { get; set; }
    public bool? IsCorrect { get; set; }
    public bool? IsDeleted { get; set; }
}

public class UpdateQuestionAnswerCommandHanler : IRequestHandler<UpdateQuestionAnswerCommand, BaseResponse<GetBriefQuestionAnswerResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateQuestionAnswerCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefQuestionAnswerResponseModel>> Handle(UpdateQuestionAnswerCommand request, CancellationToken cancellationToken)
    {
        if (request.QuestionId != null)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);
            if (question == null)
            {
                return new BaseResponse<GetBriefQuestionAnswerResponseModel>
                {
                    Success = false,
                    Message = "Question not found",
                };
            }
        }

        var questionAnswer = await _context.QuestionAnswers.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(questionAnswer == null)
        {
            return new BaseResponse<GetBriefQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "QuestionAnswer is not found",
                Errors = ["QuestionAnswer is not found"]
            };
        }

        //_mapper.Map(request, questionAnswer);
        // Use reflection to update non-null properties
        foreach (var property in request.GetType().GetProperties())
        {
            var requestValue = property.GetValue(request);
            if (requestValue != null)
            {
                var targetProperty = questionAnswer.GetType().GetProperty(property.Name);
                if (targetProperty != null)
                {
                    targetProperty.SetValue(questionAnswer, requestValue);
                }
            }
        }

        var updateQuestionAnswerResult = _context.Update(questionAnswer);

        if (updateQuestionAnswerResult.Entity == null)
        {
            return new BaseResponse<GetBriefQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "Update question answer failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionAnswerResult = _mapper.Map<GetBriefQuestionAnswerResponseModel>(updateQuestionAnswerResult.Entity);

        return new BaseResponse<GetBriefQuestionAnswerResponseModel>
        {
            Success = true,
            Message = "Update question answer successful",
            Data = mappedQuestionAnswerResult
        };
    }
}
