using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.ChallengeLevels.Commands.UpdateChallengeLevel;
public class UpdateChallengeLevelCommandsValidator : AbstractValidator<UpdateChallengeLevelCommands>
{
    public UpdateChallengeLevelCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
