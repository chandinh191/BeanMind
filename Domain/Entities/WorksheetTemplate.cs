using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class WorksheetTemplate : BaseAuditableEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Classification { get; set; }  //  0: course, 1: chapter, 2: topic 
    [ForeignKey(nameof(Course))]
    public Guid? CourseId { get; set; }
    public Course? Course { get; set; }
    [ForeignKey(nameof(Chapter))]
    public Guid? ChapterId { get; set; }
    public Chapter? Chapter { get; set; }
    [ForeignKey(nameof(Topic))]
    public Guid? TopicId { get; set; }
    public Topic? Topic { get; set; }

    public IEnumerable<Worksheet> Worksheets { get; set; }
    public IEnumerable<LevelTemplateRelation> LevelTemplateRelations { get; set; }
}
