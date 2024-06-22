using Application.Chapters;
using Application.Common;
using Application.Subjects;
using Application.Topics;
using Application.Worksheets;
using AutoMapper;
using Domain.Entities;

namespace Application.WorksheetTemplates;

[AutoMap(typeof(Domain.Entities.WorksheetTemplate))]
public class GetBriefWorksheetTemplateResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public int Classification { get; set; }  //  0: subject, 1: chapter, 2: topic 
}

[AutoMap(typeof(Domain.Entities.WorksheetTemplate))]
public class GetWorksheetTemplateResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public int Classification { get; set; }  //  0: subject, 1: chapter, 2: topic 
    public List<Worksheet> Worksheets { get; set; }
    public List<LevelTemplateRelation> LevelTemplateRelations { get; set; }
}
