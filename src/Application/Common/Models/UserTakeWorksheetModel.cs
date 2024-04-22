using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Mappings;
using BeanMind.Domain.Entities;
using BeanMind.Domain.Enums;

namespace BeanMind.Application.Common.Models;
public class UserTakeWorksheetModel : IMapFrom<UserTakeWorksheet>
{
    [ForeignKey(nameof(ApplicationUser))]
    public string ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    [ForeignKey(nameof(Worksheet))]
    public Guid WorksheetId { get; set; }
    public virtual Worksheet Worksheet { get; set; }
    public InteracStatus InteracStatus { get; set; }
    public int? Point { get; set; }
}
