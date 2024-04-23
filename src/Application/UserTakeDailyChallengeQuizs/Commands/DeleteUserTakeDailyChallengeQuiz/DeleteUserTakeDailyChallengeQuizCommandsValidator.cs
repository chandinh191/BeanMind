using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.UserTakeDailyChallengeQuizs.Commands.DeleteUserTakeDailyChallengeQuiz;
public class DeleteUserTakeDailyChallengeQuizCommandsValidator : AbstractValidator<DeleteUserTakeDailyChallengeQuizCommands>
{
    public DeleteUserTakeDailyChallengeQuizCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();  
    }
}
