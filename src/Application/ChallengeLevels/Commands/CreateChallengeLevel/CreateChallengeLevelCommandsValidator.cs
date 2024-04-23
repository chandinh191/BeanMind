using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.ChallengeLevels.Commands.CreateChallengeLevel;
public class CreateChallengeLevelCommandsValidator : AbstractValidator<CreateChallengeLevelCommands>
{
    CreateChallengeLevelCommandsValidator()
    {

    }
}
