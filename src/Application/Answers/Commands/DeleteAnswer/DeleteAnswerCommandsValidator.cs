using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Answers.Commands.DeleteAnswer;
public class DeleteAnswerCommandsValidator : AbstractValidator<DeleteAnswerCommands>
{
    public DeleteAnswerCommandsValidator()
    {

    }
}
