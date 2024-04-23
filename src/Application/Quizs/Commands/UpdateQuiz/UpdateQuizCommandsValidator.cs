using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Quizs.Commands.UpdateQuiz;
public class UpdateQuizCommandsValidator : AbstractValidator<UpdateQuizCommands>
{
    public UpdateQuizCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
