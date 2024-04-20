using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BeanMind.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public IList<UserTakeQuiz>? UserTakeQuizs { get; set; }
    public IList<UserTakeDailyChallengeQuiz>? UserTakeDailyChallengeQuizs { get; set; }
    public IList<Transaction>? Transactions { get; set; }
    public IList<UserTakeWorksheet>? UserTakeWorksheets { get; set; }

}
