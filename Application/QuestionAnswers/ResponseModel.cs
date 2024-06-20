using Application.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.QuestionAnswers;

[AutoMap(typeof(Domain.Entities.QuestionAnswer))]
public class GetBriefQuestionAnswerResponseModel : BaseResponseModel
{
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }
}

[AutoMap(typeof(Domain.Entities.QuestionAnswer))]
public class GetQuestionAnswerResponseModel : BaseResponseModel
{
 public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public Question Question { get; set; }
}
