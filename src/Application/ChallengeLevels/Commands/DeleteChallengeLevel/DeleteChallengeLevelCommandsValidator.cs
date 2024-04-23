using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.ChallengeLevels.Commands.DeleteChallengeLevel;
public class DeleteChallengeLevelCommandsValidator : AbstractValidator<DeleteChallengeLevelCommands>
{
    public DeleteChallengeLevelCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
