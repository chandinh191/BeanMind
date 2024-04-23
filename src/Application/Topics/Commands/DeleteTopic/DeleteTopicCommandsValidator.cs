using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Topics.Commands.DeleteTopic;
public class DeleteTopicCommandsValidator : AbstractValidator<DeleteTopicCommands>
{
    public DeleteTopicCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();  
    }
}
