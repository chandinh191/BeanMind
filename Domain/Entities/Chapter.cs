using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Chapter : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    [ForeignKey(nameof(Course))]
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public IEnumerable<Topic> Topics { get; set; }
    public IEnumerable<ChapterGame> ChapterGames { get; set; }
    public IEnumerable<WorksheetTemplate> WorksheetTemplates { get; set; }
}
