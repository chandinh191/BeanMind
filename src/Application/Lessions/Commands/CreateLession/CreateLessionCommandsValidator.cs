using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Lessions.Commands.CreateLession;
public class CreateLessionCommandsValidator : AbstractValidator<CreateLessionCommands>
{
    public CreateLessionCommandsValidator()
    {
    }
}
