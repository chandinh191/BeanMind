using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Domain.Entities;

namespace BeanMind.Application.Common.Models;
public class WorksheetModel
{
    public string Name { get; set; }
    public IList<WorksheetQuestion> WorksheetQuestions { get; set; }
    public IList<UserTakeWorksheet> UserTakeWorksheets { get; set; }
}
