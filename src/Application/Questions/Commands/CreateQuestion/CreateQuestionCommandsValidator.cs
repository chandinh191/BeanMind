using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Questions.Commands.CreateQuestion;
public class CreateQuestionCommandsValidator : AbstractValidator<CreateQuestionCommands>
{
    CreateQuestionCommandsValidator()
    {

    }
}
