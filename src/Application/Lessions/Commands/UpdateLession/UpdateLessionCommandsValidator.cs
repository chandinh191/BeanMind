using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Lessions.Commands.UpdateLession;
public class UpdateLessionCommandsValidator : AbstractValidator<UpdateLessionCommands>
{
    public UpdateLessionCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
