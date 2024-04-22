using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Mappings;
using BeanMind.Domain.Entities;

namespace BeanMind.Application.Common.Models;
public class AnswerModel : IMapFrom<Answer>
{
    [ForeignKey(nameof(Question))]
    public Guid QuestionId { get; set; }
    public virtual Question Question { get; set; }
    public string ContentAnswer { get; set; }
    public bool IsConrect { get; set; }
}
