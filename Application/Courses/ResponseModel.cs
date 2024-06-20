using Application.Chapters;
using Application.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.Courses;

[AutoMap(typeof(Domain.Entities.Course))]
public class GetBriefCourseResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int TotalSlot { get; set; }
    public Guid SubjectId { get; set; }
   // public Subject Subject { get; set; }
    public Guid ProgramTypeId { get; set; }
    //public ProgramType ProgramType { get; set; }
    public Guid CourseLevelId { get; set; }
    //public CourseLevel CourseLevel { get; set; }
}

[AutoMap(typeof(Domain.Entities.Course))]
public class GetCourseResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int TotalSlot { get; set; }
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; }
    public Guid ProgramTypeId { get; set; }
    public ProgramType ProgramType { get; set; }
    public Guid CourseLevelId { get; set; }
    public CourseLevel CourseLevel { get; set; }
    public List<GetBriefChapterResponseModel> Chapters { get; set; }

}
