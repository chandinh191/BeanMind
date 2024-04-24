using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Topics.Commands.CreateTopic;
public class CreateTopicCommandsValidator : AbstractValidator<CreateTopicCommands>
{
    public CreateTopicCommandsValidator()
    {

    }
}
