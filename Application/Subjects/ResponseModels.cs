using Application.Common;
using Application.Courses;
using Application.WorksheetTemplates;
using AutoMapper;
using Domain.Entities;

namespace Application.Subjects;

[AutoMap(typeof(Domain.Entities.Subject))]
public class GetBriefSubjectResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<GetBriefCourseResponseModel> Courses { get; set; }
    public DateTime Created { get; set; }
}

[AutoMap(typeof(Domain.Entities.Subject))]
public class GetSubjectResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<GetBriefCourseResponseModel> Courses { get; set; }
    public List<GetBriefWorksheetTemplateResponseModel> WorksheetTemplates { get; set; }
    public DateTime Created { get; set; }
}

