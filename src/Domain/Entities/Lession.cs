using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Lession : BaseAuditableEntity
{
    [ForeignKey(nameof(Topic))]
    public Guid TopicId { get; set; }
    public virtual Topic? Topic { get; set; }


    public string Title { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Order { get; set; }


    public IList<Activity>? Activities { get; set; }

}
