using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Activity : BaseAuditableEntity
{
    [ForeignKey(nameof(Lession))]
    public Guid LessionId { get; set; }
    public virtual Lession? Lession { get; set; }



    [ForeignKey(nameof(Quiz))]
    public Guid? QuizId { get; set; }
    public virtual Quiz? Quiz { get; set; }

}
