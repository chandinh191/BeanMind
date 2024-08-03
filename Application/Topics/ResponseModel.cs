
using Application.Chapters;
using Application.Common;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.Questions;
using Application.WorksheetTemplates;
using Application.Processions;

namespace Application.Topics;


[AutoMap(typeof(Domain.Entities.Topic))]
public class GetBriefTopicResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int? Order { get; set; }
    public Guid ChapterId { get; set; }
    public GetBriefChapterResponseModel Chapter { get; set; }
}

[AutoMap(typeof(Domain.Entities.Topic))]
public class GetTopicResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int? Order { get; set; }
    public Guid ChapterId { get; set; }
    public GetBriefChapterResponseModel Chapter { get; set; }
    public List<GetBriefQuestionResponseModel> Questions { get; set; }
    public List<GetBriefWorksheetTemplateResponseModel> WorksheetTemplates { get; set; }
    public List<GetBriefProcessionResponseModel> Processions { get; set; }
}
