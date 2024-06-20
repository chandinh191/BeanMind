using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class QuestionAnswer : BaseAuditableEntity
{
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    [Required]
    [ForeignKey(nameof(Question))]
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
}
