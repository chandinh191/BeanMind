using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class QuestionAnswer : BaseAuditableEntity
{
    [Required]
    [ForeignKey(nameof(Question))]
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public IEnumerable<WorksheetAttemptAnswer> WorksheetAttemptAnswers { get; set; }
}
