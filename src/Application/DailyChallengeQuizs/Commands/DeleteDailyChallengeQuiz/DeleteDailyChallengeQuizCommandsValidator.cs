using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.DailyChallengeQuizs.Commands.DeleteDailyChallengeQuiz;
public class DeleteDailyChallengeQuizCommandsValidator : AbstractValidator<DeleteDailyChallengeQuizCommands>
{
    public DeleteDailyChallengeQuizCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
