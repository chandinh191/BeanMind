using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.DailyChallenges.Commands.CreateDailyChallenge;
public class CreateDailyChallengeCommandsValidator : AbstractValidator<CreateDailyChallengeCommands>
{
    public CreateDailyChallengeCommandsValidator()
    {

    }
}
