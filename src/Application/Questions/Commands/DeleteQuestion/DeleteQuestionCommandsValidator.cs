using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Questions.Commands.DeleteQuestion;
public class DeleteQuestionCommandsValidator : AbstractValidator<DeleteQuestionCommands>
{
    public DeleteQuestionCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();  
    }
}
