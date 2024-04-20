using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class DailyChallengeQuestion : BaseAuditableEntity
{
    [ForeignKey(nameof(QuestionBank))]
    public Guid QuestionBankId { get; set; }
    public virtual QuestionBank QuestionBank { get; set; }

    [ForeignKey(nameof(DailyChallengeQuiz))]
    public Guid DailyChallengeQuizId { get; set; }
    public virtual DailyChallengeQuiz DailyChallengeQuiz { get; set; }
}
