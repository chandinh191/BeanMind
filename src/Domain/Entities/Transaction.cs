using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Transaction :BaseAuditableEntity
{
    [ForeignKey(nameof(ApplicationUser))]
    public string ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    
    public int Money { get; set; }
    public int Balance { get; set; }
}
