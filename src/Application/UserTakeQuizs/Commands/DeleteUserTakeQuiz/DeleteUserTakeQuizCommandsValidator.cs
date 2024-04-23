using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.UserTakeQuizs.Commands.DeleteUserTakeQuiz;
public class DeleteUserTakeQuizCommandsValidator : AbstractValidator<DeleteUserTakeQuizCommands>
{
    public DeleteUserTakeQuizCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();  
    }
}
