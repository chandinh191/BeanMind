using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Topic : BaseAuditableEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? Order { get; set; }
    [Required]
    [ForeignKey(nameof(Chapter))]
    public Guid ChapterId { get; set; }
    public Chapter Chapter { get; set; }
    public IEnumerable<Question> Questions { get; set; }
    public IEnumerable<WorksheetTemplate> WorksheetTemplates { get; set; }
    public IEnumerable<Procession> Processions { get; set; }

}
