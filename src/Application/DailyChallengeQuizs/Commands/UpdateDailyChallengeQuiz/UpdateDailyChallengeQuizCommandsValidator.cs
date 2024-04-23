using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.DailyChallengeQuizs.Commands.UpdateDailyChallengeQuiz;
public class UpdateDailyChallengeQuizCommandsValidator : AbstractValidator<UpdateDailyChallengeQuizCommands>
{
    public UpdateDailyChallengeQuizCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
