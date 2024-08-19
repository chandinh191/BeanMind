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
    public bool IsDeleted { get; set; } = false;
    public DateTime Created { get; set; }
}

[AutoMap(typeof(Domain.Entities.QuestionAnswer))]
public class GetQuestionAnswerResponseModel : BaseResponseModel
{
    public Guid QuestionId { get; set; }
    public GetBriefQuestionResponseModel Question { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsDeleted { get; set; } = false;
    public List<GetBriefWorksheetAttemptAnswerResponseModel> WorksheetAttemptAnswers { get; set; }
    public DateTime Created { get; set; }
}
