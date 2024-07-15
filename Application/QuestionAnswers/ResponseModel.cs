using Application.Common;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.Questions;
using Application.WorksheetAttemptAnswers;

namespace Application.QuestionAnswers;

[AutoMap(typeof(Domain.Entities.QuestionAnswer))]
public class GetBriefQuestionAnswerResponseModel : BaseResponseModel
{
    public Guid QuestionId { get; set; }
    public GetBriefQuestionResponseModel Question { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
   
}

[AutoMap(typeof(Domain.Entities.QuestionAnswer))]
public class GetQuestionAnswerResponseModel : BaseResponseModel
{
    public Guid QuestionId { get; set; }
    public GetBriefQuestionResponseModel Question { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public IEnumerable<GetBriefWorksheetAttemptAnswerResponseModel> WorksheetAttemptAnswers { get; set; }
}
