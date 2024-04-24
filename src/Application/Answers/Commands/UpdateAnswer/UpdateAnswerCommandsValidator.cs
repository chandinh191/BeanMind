using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace BeanMind.Application.Answers.Commands.UpdateAnswer;
public class UpdateAnswerCommandsValidator : AbstractValidator<UpdateAnswerCommands>
{
    public UpdateAnswerCommandsValidator()
    {

    }
}