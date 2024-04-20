using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class UserTakeDailyChallengeQuiz : BaseAuditableEntity
{
    [ForeignKey(nameof(ApplicationUser))]
    public string ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }

    [ForeignKey(nameof(DailyChallengeQuiz))]
    public Guid DailyChallengeQuizId { get; set; }
    public virtual DailyChallengeQuiz DailyChallengeQuiz { get; set; }

    public bool IsCompleted { get; set; }
    public int Point {  get; set; }
}
