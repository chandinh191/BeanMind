using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Topic :BaseAuditableEntity
{
    [ForeignKey(nameof(Subject))]
    public Guid SubjectId { get; set; }
    public virtual Subject? Subject { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageURL { get; set; }
    public bool Status { get; set; }
    public IList<Lession>? Lessions { get; set; }
}
