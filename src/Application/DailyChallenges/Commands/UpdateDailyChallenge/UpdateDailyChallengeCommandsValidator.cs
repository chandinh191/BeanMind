using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.DailyChallenges.Commands.UpdateDailyChallenge;
public class UpdateDailyChallengeCommandsValidator : AbstractValidator<UpdateDailyChallengeCommands>
{
    public UpdateDailyChallengeCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
