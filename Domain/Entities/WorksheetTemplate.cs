using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class WorksheetTemplate : BaseAuditableEntity
{
    public string Title { get; set; }   //  0: subject, 1: chapter, 2: topic 
    public IEnumerable<Worksheet> Worksheets { get; set; }
    public IEnumerable<LevelTemplateRelation> LevelTemplateRelations { get; set; }
}
