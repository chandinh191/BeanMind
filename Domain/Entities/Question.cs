using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Question : BaseAuditableEntity
{
    public string Content { get; set; }
    public string? ImageUrl { get; set; }
    [Required]
    [ForeignKey(nameof(Entities.Topic))]
    public Guid TopicId { get; set; }
    public Topic Topic { get; set; }

    [Required]
    [ForeignKey(nameof(QuestionLevel))]
    public Guid QuestionLevelId { get; set; }
    public QuestionLevel QuestionLevel { get; set; }


    public IEnumerable<QuestionAnswer> QuestionAnswers { get; set; }
    public IEnumerable<WorksheetQuestion> WorksheetQuestions { get; set; }
}

