using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.ActivityTypes.Commands.CreateActivityType;
using FluentValidation;

namespace BeanMind.Application.Quizs.Commands.CreateQuiz;
public class CreateQuizCommandsValidator : AbstractValidator<CreateQuizCommands>
{
    public CreateQuizCommandsValidator()
    {
    }
}
