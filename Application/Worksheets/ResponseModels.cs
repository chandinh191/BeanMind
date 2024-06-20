using Application.Common;
using Application.WorksheetTemplates;
using AutoMapper;

namespace Application.Worksheets;


[AutoMap(typeof(Domain.Entities.Worksheet))]
public class GetBriefWorksheetResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid ActivityId { get; set; }
    public Guid WorksheetTemplateId { get; set; }
}

[AutoMap(typeof(Domain.Entities.Worksheet))]
public class GetWorksheetResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }

    public GetBriefWorksheetTemplateResponseModel WorksheetTemplate { get; set; }

}
