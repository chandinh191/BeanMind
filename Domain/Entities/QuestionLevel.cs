using Domain.Common;

namespace Domain.Entities;

public class QuestionLevel : BaseAuditableEntity
{
    public string Title { get; set; }
    public IEnumerable<Question> Questions { get; set; }
    public IEnumerable<LevelTemplateRelation> LevelTemplateRelations { get; set; }
}
