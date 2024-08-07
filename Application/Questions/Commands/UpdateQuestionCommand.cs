using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.Questions.Commands;
public class UpdateQuestionAnswerModel
{
/*    [Required]
    public Guid Id { get; set; }*/
    [Required]
    public string Content { get; set; }
    [Required]
    public bool IsCorrect { get; set; }
}

[AutoMap(typeof(Domain.Entities.Question), ReverseMap = true)]
public sealed record UpdateQuestionCommand : IRequest<BaseResponse<GetBriefQuestionResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? TopicId { get; set; }
    public Guid? QuestionLevelId { get; set; }
    public List<UpdateQuestionAnswerModel>? QuestionAnswers { get; set; }
}

public class UpdateQuestionCommandHanler : IRequestHandler<UpdateQuestionCommand, BaseResponse<GetBriefQuestionResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefQuestionResponseModel>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        if (request.TopicId != null)
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == request.TopicId);
            if (topic == null)
            {
                return new BaseResponse<GetBriefQuestionResponseModel>
                {
                    Success = false,
                    Message = "Topic not found",
                };
            }
        }
        if (request.QuestionLevelId != null)
        {
            var questionLevel = await _context.QuestionLevels.FirstOrDefaultAsync(x => x.Id == request.QuestionLevelId);
            if (questionLevel == null)
            {
                return new BaseResponse<GetBriefQuestionResponseModel>
                {
                    Success = false,
                    Message = "Question level not found",
                };
            }
        }

        var question = await _context.Questions
            .Include(o => o.QuestionAnswers)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if(question == null)
        {
            return new BaseResponse<GetBriefQuestionResponseModel>
            {
                Success = false,
                Message = "Question is not found",
                Errors = ["Question is not found"]
            };
        }

        //_mapper.Map(request, question);
        // Use reflection to update non-null properties
        foreach (var property in request.GetType().GetProperties())
        {
            var requestValue = property.GetValue(request);
            if (requestValue != null)
            {
                var targetProperty = question.GetType().GetProperty(property.Name);
                if (targetProperty != null && targetProperty.Name != "QuestionAnswers")
                {
                    targetProperty.SetValue(question, requestValue);
                }
            }
        }

        var updateQuestionResult = _context.Update(question);

        if (updateQuestionResult.Entity == null)
        {
            return new BaseResponse<GetBriefQuestionResponseModel>
            {
                Success = false,
                Message = "Update question failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        //Update Question Answer (if any)
        if (request.QuestionAnswers != null && request.QuestionAnswers.Count > 0)
        {
            var questionAnswerRetrieves = _context.QuestionAnswers.Where(o => o.QuestionId == question.Id).AsQueryable();
            foreach (var questionAnswerRetrieve in questionAnswerRetrieves)
            {
                questionAnswerRetrieve.IsDeleted = true;
            }
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var questionAnswer in request.QuestionAnswers)
            {
                var questionAnswerModel = new QuestionAnswer
                {
                    QuestionId = question.Id,
                    Content = questionAnswer.Content,
                    IsCorrect = questionAnswer.IsCorrect,
                };
                var existedRecord = question.QuestionAnswers.FirstOrDefault(x => x.Content == questionAnswer.Content);
                if (existedRecord != null) {
                    existedRecord=questionAnswerModel;
                    existedRecord.IsDeleted = false;
                    _context.Update(existedRecord);
                }
                else
                {
                    var createQuestionAnswerResult = await _context.AddAsync(questionAnswerModel, cancellationToken);
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        question = await _context.Questions
            .FirstOrDefaultAsync(x => x.Id == request.Id);
        if (question != null)
        {
            question.QuestionAnswers = question.QuestionAnswers.Where(qa => !qa.IsDeleted).ToList();
        }

        var mappedQuestionResult = _mapper.Map<GetBriefQuestionResponseModel>(question);

        return new BaseResponse<GetBriefQuestionResponseModel>
        {
            Success = true,
            Message = "Update question successful",
            Data = mappedQuestionResult
        };
    }
}
