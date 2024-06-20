using Application.Common;
using Application.QuestionAnswers;
using Application.QuestionLevels;

using Application.Topics;

using AutoMapper;

namespace Application.Questions;

[AutoMap(typeof(Domain.Entities.Question))]
public class GetBriefQuestionResponseModel : BaseResponseModel
{
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public Guid TopicId { get; set; }
    public Guid QuestionLevelId { get; set; }
    public Guid QuestionTypeId { get; set; }
}

[AutoMap(typeof(Domain.Entities.Question))]
public class GetQuestionResponseModel : BaseResponseModel
{
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public GetBriefTopicResponseModel Topic { get; set; }
    public GetBriefQuestionLevelResponseModel QuestionLevel { get; set; }
    public IEnumerable<GetBriefQuestionAnswerResponseModel> QuestionAnswers { get; set; }
}
