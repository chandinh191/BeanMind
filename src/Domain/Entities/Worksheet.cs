using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Worksheet :BaseAuditableEntity
{
    public string Name { get; set; }

 
    public IList<WorksheetQuestion> WorksheetQuestions { get; set; }
    public IList<UserTakeWorksheet> UserTakeWorksheets { get; set; }
}
