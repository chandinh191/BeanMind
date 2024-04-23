using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Topics.Commands.DeleteTopic;
public class DeleteTopicCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteTopicCommandsHandler : IRequestHandler<DeleteTopicCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteTopicCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteTopicCommands request, CancellationToken cancellationToken)
    {
        var topic = await _context.Get<Domain.Entities.Topic>().FindAsync(request.Id);
        if (topic == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Topic), request.Id);
        }

        topic.IsDeleted = true;
        _context.Get<Domain.Entities.Topic>().Update(topic);
        await _context.SaveChangesAsync(cancellationToken);

        return topic.Id;
    }
}