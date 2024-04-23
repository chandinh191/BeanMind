using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.ActivityTypes.Commands.DeleteActivityType;
public class DeleteActivityTypeCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteActivityTypeCommandsHandler : IRequestHandler<DeleteActivityTypeCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteActivityTypeCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteActivityTypeCommands request, CancellationToken cancellationToken)
    {
        var activityType = _context.Get<Domain.Entities.ActivityType>().FirstOrDefault(x => x.Id == request.Id);

        if (activityType == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.ActivityType), request.Id);
        }

        _context.Get<Domain.Entities.ActivityType>().Remove(activityType);
        await _context.SaveChangesAsync(cancellationToken);

        return activityType.Id;
    }
}
