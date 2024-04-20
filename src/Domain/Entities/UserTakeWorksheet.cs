using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class UserTakeWorksheet :BaseAuditableEntity
{
    [ForeignKey(nameof(ApplicationUser))]
    public string ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }

    [ForeignKey(nameof(Worksheet))]
    public Guid WorksheetId { get; set; }
    public virtual Worksheet Worksheet { get; set; }


    public InteracStatus InteracStatus { get; set; }
    public int? Point {  get; set; }
}
