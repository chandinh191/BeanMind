using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Questions.Commands.UpdateQuestion;
public class UpdateQuestionCommandsValidator : AbstractValidator<UpdateQuestionCommands>
{
    public UpdateQuestionCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
