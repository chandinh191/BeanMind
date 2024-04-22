using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Mappings;
using BeanMind.Domain.Entities;

namespace BeanMind.Application.Common.Models;
public class DailyChallengeQuestionModel : IMapFrom<DailyChallengeQuestion>
{
    [ForeignKey(nameof(QuestionBank))]
    public Guid QuestionBankId { get; set; }
    public virtual QuestionBank QuestionBank { get; set; }

    [ForeignKey(nameof(DailyChallengeQuiz))]
    public Guid DailyChallengeQuizId { get; set; }
    public virtual DailyChallengeQuiz DailyChallengeQuiz { get; set; }
}
