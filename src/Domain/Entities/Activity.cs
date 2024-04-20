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

    [ForeignKey(nameof(Document))]
    public Guid? DocumentId { get; set; }
    public virtual Document? Document { get; set; }

    [ForeignKey(nameof(Video))]
    public Guid? VideoId { get; set; }
    public virtual Video? Video { get; set; }

}
