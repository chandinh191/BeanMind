using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Topics.Commands.UpdateTopic;
public class UpdateTopicCommandsValidator : AbstractValidator<UpdateTopicCommands>
{
    public UpdateTopicCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
