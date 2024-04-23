using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Videos.Commands.DeleteVideo;
public class DeleteVideoCommandsValidator : AbstractValidator<DeleteVideoCommands>
{
    public DeleteVideoCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();  
    }
}
