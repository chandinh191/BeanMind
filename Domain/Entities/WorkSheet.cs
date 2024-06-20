using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Worksheet : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }

    [ForeignKey(nameof(WorksheetTemplate))]
    public Guid? WorksheetTemplateId { get; set; }
    public WorksheetTemplate? WorksheetTemplate { get; set; }

    public IEnumerable<WorksheetQuestion> WorksheetQuestions { get; set; }
}
