using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Mappings;
using BeanMind.Domain.Entities;

namespace BeanMind.Application.Common.Models;
public class DailyChallengedModel : IMapFrom<DailyChallenge>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<DailyChallengeQuiz>? DailyChallengeQuizs { get; set; }
}
