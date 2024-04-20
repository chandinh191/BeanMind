using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Quiz : BaseAuditableEntity
{
/*    [ForeignKey(nameof(Activity))]
    public Guid ActivityId { get; set; }
    public virtual Activity? Activity { get; set; }*/



    public IList<UserTakeQuiz>? UserTakeQuizs { get; set; }
}
