using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Quizs.Commands.DeleteQuiz;
public class DeleteQuizCommandsValidator : AbstractValidator<DeleteQuizCommands>
{
    public DeleteQuizCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();  
    }
}
