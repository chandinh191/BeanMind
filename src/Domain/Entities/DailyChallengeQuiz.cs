using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class DailyChallengeQuiz : BaseAuditableEntity
{
    public string Name { get; set; }
    public QuestionLevel Level { get; set; }
    public IList<DailyChallengeQuestion> DailyChallengeQuestions { get; set; }
    public IList<UserTakeDailyChallengeQuiz> UserTakeDailyChallengeQuizs { get; set; }


    [ForeignKey(nameof(DailyChallenge))]
    public Guid DailyChallengeId { get; set; }
    public virtual DailyChallenge DailyChallenge { get; set; }
}
