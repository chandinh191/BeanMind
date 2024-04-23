using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Topics.Commands.UpdateTopic;
public class UpdateTopicCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateTopicCommandsHandler : IRequestHandler<UpdateTopicCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateTopicCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateTopicCommands request, CancellationToken cancellationToken)
    {
        var topic = await _context.Get<Domain.Entities.Topic>().FindAsync(request.Id);
        if (topic == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Topic), request.Id);
        }

        _context.Get<Domain.Entities.Topic>().Update(topic);
        await _context.SaveChangesAsync(cancellationToken);

        return topic.Id;
    }
}