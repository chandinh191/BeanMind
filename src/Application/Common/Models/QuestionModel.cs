using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Domain.Entities;

namespace BeanMind.Application.Common.Models;
public class QuestionModel
{
    [ForeignKey(nameof(Quiz))]
    public Guid QuizId { get; set; }
    public virtual Quiz? Quiz { get; set; }
    public string ContentQuestion { get; set; }
    public IList<Answer>? Answers { get; set; }
}
