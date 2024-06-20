using Application.Common;
using Application.Topics;
using AutoMapper;
using Domain.Entities;

namespace Application.Chapters;


[AutoMap(typeof(Domain.Entities.Chapter))]
public class GetBriefChapterResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CourseId { get; set; }
}

[AutoMap(typeof(Domain.Entities.Chapter))]
public class GetChapterResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Course Course { get; set; }
    public List<GetBriefTopicResponseModel> Topics { get; set; }
}
