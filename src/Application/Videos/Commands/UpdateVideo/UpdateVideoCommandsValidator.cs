using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Videos.Commands.UpdateVideo;
public class UpdateVideoCommandsValidator : AbstractValidator<UpdateVideoCommands>
{
    public UpdateVideoCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
