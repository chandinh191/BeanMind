using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class DailyChallenge : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<DailyChallengeQuiz>? DailyChallengeQuizs { get; set; }
}
