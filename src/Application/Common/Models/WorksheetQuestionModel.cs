using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Mappings;
using BeanMind.Domain.Entities;

namespace BeanMind.Application.Common.Models;
public class WorksheetQuestionModel : IMapFrom<WorksheetQuestion>
{
    [ForeignKey(nameof(QuestionBank))]
    public Guid QuestionBankId { get; set; }
    public virtual QuestionBank QuestionBank { get; set; }
    [ForeignKey(nameof(Worksheet))]
    public Guid WorksheetId { get; set; }
    public virtual Worksheet Worksheet { get; set; }
}
