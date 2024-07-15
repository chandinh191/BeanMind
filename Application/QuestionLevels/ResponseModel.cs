using Application.Common;
using Application.LevelTemplateRelations;
using Application.Questions;
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
    public IEnumerable<GetBriefQuestionResponseModel> Questions { get; set; }
    public IEnumerable<GetBriefLevelTemplateRelationResponseModel> LevelTemplateRelation { get; set; }
}

