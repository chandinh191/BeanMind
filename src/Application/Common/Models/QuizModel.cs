using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Domain.Entities;

namespace BeanMind.Application.Common.Models;
public class QuizModel
{
    [ForeignKey(nameof(Activity))]
    public Guid ActivityId { get; set; }
    public virtual Activity? Activity { get; set; }
    public IList<Question>? Questions { get; set; }
    public IList<UserTakeQuiz>? UserTakeQuizs { get; set; }
}
