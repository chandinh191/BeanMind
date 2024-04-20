using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Document : BaseAuditableEntity
{
    public string Description { get; set; }
}
