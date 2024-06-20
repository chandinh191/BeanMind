
using Application.Chapters;
using Application.Common;
using AutoMapper;

namespace Application.Topics;


[AutoMap(typeof(Domain.Entities.Topic))]
public class GetBriefTopicResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid ChapterId { get; set; }
}

[AutoMap(typeof(Domain.Entities.Topic))]
public class GetTopicResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public GetBriefChapterResponseModel Chapter { get; set; }
}
