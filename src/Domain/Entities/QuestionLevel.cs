using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class QuestionLevel : BaseAuditableEntity
{
    public string Name { get; set; }
    public string DifficultLevel { get; set; }
    public int LearningPoint { get; set; }
    public IList<QuestionBank>? QuestionBanks { get; set; }
}   
