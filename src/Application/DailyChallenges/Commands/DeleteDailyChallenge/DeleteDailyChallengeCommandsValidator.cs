using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.DailyChallenges.Commands.DeleteDailyChallenge;
public class DeleteDailyChallengeCommandsValidator : AbstractValidator<DeleteDailyChallengeCommands>
{
    public DeleteDailyChallengeCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
