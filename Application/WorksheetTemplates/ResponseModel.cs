using Application.Chapters;
using Application.Common;
using Application.Courses;
using Application.LevelTemplateRelations;
using Application.Subjects;
using Application.Topics;
using Application.Worksheets;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.WorksheetTemplates;

[AutoMap(typeof(Domain.Entities.WorksheetTemplate))]
public class GetBriefWorksheetTemplateResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public int Classification { get; set; }  //  0: course, 1: chapter, 2: topic 
    public Guid? CourseId { get; set; }
    //public GetBriefCourseResponseModel? Course { get; set; }
    public Guid? ChapterId { get; set; }
    //public GetBriefChapterResponseModel? Chapter { get; set; }
    public Guid? TopicId { get; set; }
    //public GetBriefTopicResponseModel? Topic { get; set; }
}

[AutoMap(typeof(Domain.Entities.WorksheetTemplate))]
public class GetWorksheetTemplateResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public int Classification { get; set; }  //  0: course, 1: chapter, 2: topic 
    public Guid? CourseId { get; set; }
    public GetBriefCourseResponseModel? Course { get; set; }
    public Guid? ChapterId { get; set; }
    public GetBriefChapterResponseModel? Chapter { get; set; }
    public Guid? TopicId { get; set; }
    public GetBriefTopicResponseModel? Topic { get; set; }

    public List<GetBriefWorksheetResponseModel> Worksheets { get; set; }
    public List<GetBriefLevelTemplateRelationResponseModel> LevelTemplateRelations { get; set; }
}
