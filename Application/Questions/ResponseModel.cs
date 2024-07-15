using Application.Common;
using Application.QuestionAnswers;
using Application.QuestionLevels;

using Application.Topics;

using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.WorksheetQuestions;

namespace Application.Questions;

[AutoMap(typeof(Domain.Entities.Question))]
public class GetBriefQuestionResponseModel : BaseResponseModel
{
    public string Content { get; set; }
    public string? ImageUrl { get; set; }
    public Guid TopicId { get; set; }
    public GetBriefTopicResponseModel Topic { get; set; }
    public Guid QuestionLevelId { get; set; }
    public GetBriefQuestionLevelResponseModel QuestionLevel { get; set; }
}

[AutoMap(typeof(Domain.Entities.Question))]
public class GetQuestionResponseModel : BaseResponseModel
{
    public string Content { get; set; }
    public string? ImageUrl { get; set; }
    public Guid TopicId { get; set; }
    public GetBriefTopicResponseModel Topic { get; set; }
    public Guid QuestionLevelId { get; set; }
    public GetBriefQuestionLevelResponseModel QuestionLevel { get; set; }
    public List<GetBriefQuestionAnswerResponseModel> QuestionAnswers { get; set; }
    public List<GetBriefWorksheetQuestionResponseModel> WorksheetQuestions { get; set; }
}
