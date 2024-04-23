using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.UserTakeQuizs.Commands.CreateUserTakeQuiz;
public class CreateUserTakeQuizCommandsValidator : AbstractValidator<CreateUserTakeQuizCommands>
{
    public CreateUserTakeQuizCommandsValidator()
    {

    }
}
