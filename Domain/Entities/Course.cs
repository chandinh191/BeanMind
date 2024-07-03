using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Course : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int TotalSlot { get; set; }
    [Required]
    [ForeignKey(nameof(Subject))]
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; }
    [Required]
    [ForeignKey(nameof(ProgramType))]
    public Guid ProgramTypeId { get; set; }
    public ProgramType ProgramType { get; set; }
    [Required]
    [ForeignKey(nameof(CourseLevel))]
    public Guid CourseLevelId { get; set; }
    public CourseLevel CourseLevel { get; set; }
    public IEnumerable<TeachingSlot> TeachingSlots { get; set; }
    public IEnumerable<Teachable> Teachables { get; set; }
    public IEnumerable<Chapter> Chapters { get; set; }
    public IEnumerable<Enrollment> Enrollments { get; set; }
    public IEnumerable<WorksheetTemplate>? WorksheetTemplates { get; set; }

}
