using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Answer : BaseAuditableEntity
{
    [ForeignKey(nameof(Question))]
    public Guid QuestionId { get; set; }
    public virtual Question Question { get; set; }

    public string ContentAnswer { get; set; }
    public bool IsConrect {  get; set; }
}
