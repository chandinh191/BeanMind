using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Topics.Commands.CreateTopic;
public class CreateTopicCommands : IRequest<Guid>
{

}

public class CreateTopicCommandsHandler : IRequestHandler<CreateTopicCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTopicCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateTopicCommands request, CancellationToken cancellationToken)
    {
        var topic = new Domain.Entities.Topic
        {

        };

        _context.Get<Domain.Entities.Topic>().Add(topic);
        await _context.SaveChangesAsync(cancellationToken);
        return topic.Id;
    }
}
