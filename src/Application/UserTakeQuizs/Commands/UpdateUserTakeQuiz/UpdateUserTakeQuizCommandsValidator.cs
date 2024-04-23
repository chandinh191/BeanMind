using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.UserTakeQuizs.Commands.UpdateUserTakeQuiz;
public class UpdateUserTakeQuizCommandsValidator : AbstractValidator<UpdateUserTakeQuizCommands>
{
    public UpdateUserTakeQuizCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
