using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Subject : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual IList<Topic>? Topics { get; set; }
}
