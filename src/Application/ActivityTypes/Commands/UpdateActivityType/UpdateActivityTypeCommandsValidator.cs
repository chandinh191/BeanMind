using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.ActivityTypes.Commands.UpdateActivityType;
public class UpdateActivityTypeCommandsValidator : AbstractValidator<UpdateActivityTypeCommands>
{
    UpdateActivityTypeCommandsValidator()
    {

    }
}
