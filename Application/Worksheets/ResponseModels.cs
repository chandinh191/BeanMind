using Application.Common;
using Application.WorksheetAttempts;
using Application.WorksheetQuestions;
using Application.WorksheetTemplates;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Worksheets;


[AutoMap(typeof(Domain.Entities.Worksheet))]
public class GetBriefWorksheetResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid? WorksheetTemplateId { get; set; }
    public GetBriefWorksheetTemplateResponseModel? WorksheetTemplate { get; set; }
    public List<GetBriefWorksheetQuestionResponseModel> WorksheetQuestions { get; set; }

}

[AutoMap(typeof(Domain.Entities.Worksheet))]
public class GetWorksheetResponseModel : BaseResponseModel
{    public string Title { get; set; }
    public string Description { get; set; }
    public Guid? WorksheetTemplateId { get; set; }
    public GetBriefWorksheetTemplateResponseModel? WorksheetTemplate { get; set; }
    public List<GetBriefWorksheetQuestionResponseModel> WorksheetQuestions { get; set; }
    public List<GetBriefWorksheetAttemptResponseModel> WorksheetAttempts { get; set; }

}
