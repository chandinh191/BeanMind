using Domain.Common;

namespace Domain.Entities;

public class Subject : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IEnumerable<Course> Courses { get; set; }
    public IEnumerable<WorksheetTemplate> WorksheetTemplates { get; set; }
}
