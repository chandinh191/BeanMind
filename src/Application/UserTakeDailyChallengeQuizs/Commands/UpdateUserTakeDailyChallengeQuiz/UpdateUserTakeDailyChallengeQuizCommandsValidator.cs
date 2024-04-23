using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.UserTakeDailyChallengeQuizs.Commands.UpdateUserTakeDailyChallengeQuiz;
public class UpdateUserTakeDailyChallengeQuizCommandsValidator : AbstractValidator<UpdateUserTakeDailyChallengeQuizCommands>
{
    public UpdateUserTakeDailyChallengeQuizCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
