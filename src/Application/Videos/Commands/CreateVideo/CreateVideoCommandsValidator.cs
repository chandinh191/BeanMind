using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Videos.Commands.CreateVideo;
public class CreateVideoCommandsValidator : AbstractValidator<CreateVideoCommands>
{
    public CreateVideoCommandsValidator()
    {

    }
}
