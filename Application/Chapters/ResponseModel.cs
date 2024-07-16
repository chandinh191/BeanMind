using Application.ChapterGames;
using Application.Common;
using Application.Courses;
using Application.Topics;
using Application.WorksheetTemplates;
using AutoMapper;
using Domain.Entities;

namespace Application.Chapters;


[AutoMap(typeof(Domain.Entities.Chapter))]
public class GetBriefChapterResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CourseId { get; set; }
    //public GetBriefCourseResponseModel Course { get; set; }
}

[AutoMap(typeof(Domain.Entities.Chapter))]
public class GetChapterResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CourseId { get; set; }
    public GetBriefCourseResponseModel Course { get; set; }
    public List<GetBriefTopicResponseModel> Topics { get; set; }
    public List<GetBriefChapterGameResponseModel> ChapterGames { get; set; }
    public List<GetBriefWorksheetTemplateResponseModel> WorksheetTemplates { get; set; }
}
