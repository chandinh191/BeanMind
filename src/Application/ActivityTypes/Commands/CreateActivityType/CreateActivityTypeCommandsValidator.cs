using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.ActivityTypes.Commands.CreateActivityType;
public class CreateActivityTypeCommandsValidator : AbstractValidator<CreateActivityTypeCommands>
{
    public CreateActivityTypeCommandsValidator()
    {

    }
}
