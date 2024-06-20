using Application.Common;
using Application.Courses;
using AutoMapper;

namespace Application.Subjects;

[AutoMap(typeof(Domain.Entities.Subject))]
public class GetBriefSubjectResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
}

[AutoMap(typeof(Domain.Entities.Subject))]
public class GetSubjectResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<GetBriefCourseResponseModel> Courses { get; set; }
}

