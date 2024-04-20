using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class QuestionBank : BaseAuditableEntity
{
    public string Question {  get; set; }
    public string Answer1 { get; set; }
    public string Answer2 { get; set; }
    public string Answer3 { get; set; }
    public string Answer4 { get; set; }
    public int CorrectAnswer { get; set; }
    public QuestionLevel Level { get; set; }
    public IList<DailyChallengeQuestion> DailyChallengeQuestions { get; set; }
    public IList<WorksheetQuestion> WorksheetQuestions { get; set; }
}
