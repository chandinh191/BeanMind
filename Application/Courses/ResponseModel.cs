using Application.Chapters;
using Application.Common;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.Subjects;
using Application.ProgramTypes;
using Application.CourseLevels;
using Application.Teachables;
using Application.TeachingSlots;
using Application.Enrollments;
using Application.WorksheetTemplates;

namespace Application.Courses;

[AutoMap(typeof(Domain.Entities.Course))]
public class GetBriefCourseResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int TotalSlot { get; set; }
    public Guid SubjectId { get; set; }
    //public GetBriefSubjectResponseModel Subject { get; set; }
    public Guid ProgramTypeId { get; set; }
    //public GetBriefProgramTypeResponseModel ProgramType { get; set; }
    public Guid CourseLevelId { get; set; }
    //public GetBriefCourseLevelResponseModel CourseLevel { get; set; }
}

[AutoMap(typeof(Domain.Entities.Course))]
public class GetCourseResponseModel : BaseResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int TotalSlot { get; set; }
    public Guid SubjectId { get; set; }
    public GetBriefSubjectResponseModel Subject { get; set; }
    public Guid ProgramTypeId { get; set; }
    public GetBriefProgramTypeResponseModel ProgramType { get; set; }
    public Guid CourseLevelId { get; set; }
    public GetBriefCourseLevelResponseModel CourseLevel { get; set; }
    public List<GetBriefTeachingSlotResponseModel> TeachingSlots { get; set; }
    public List<GetBriefTeachableResponseModel> Teachables { get; set; }
    public List<GetBriefChapterResponseModel> Chapters { get; set; }
    public List<GetBriefEnrollmentResponseModel> Enrollments { get; set; }
    public List<GetBriefWorksheetTemplateResponseModel>? WorksheetTemplates { get; set; }

}
