using Application.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.QuestionLevels;

[AutoMap(typeof(Domain.Entities.QuestionLevel))]
public class GetBriefQuestionLevelResponseModel : BaseResponseModel
{
    public string Title { get; set; }
}

[AutoMap(typeof(Domain.Entities.QuestionLevel))]
public class GetQuestionLevelResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public List<Question> Questions { get; set; }
}

