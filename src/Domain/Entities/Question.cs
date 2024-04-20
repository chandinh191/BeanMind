using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Question : BaseAuditableEntity
{
    [ForeignKey(nameof(Quiz))]
    public Guid QuizId { get; set; }
    public virtual Quiz? Quiz { get; set; }


    public string ContentQuestion { get; set; }
    public IList<Answer>? Answers { get; set; }
}
