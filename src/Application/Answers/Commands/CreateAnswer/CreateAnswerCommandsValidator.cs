using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Answers.Commands.CreateAnswer;
public class CreateAnswerCommandsValidator : AbstractValidator<CreateAnswerCommands>
{
    public CreateAnswerCommandsValidator()
    {

    }
}
