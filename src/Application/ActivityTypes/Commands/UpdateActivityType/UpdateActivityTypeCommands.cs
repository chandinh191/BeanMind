using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.ActivityTypes.Commands.UpdateActivityType;
public class UpdateActivityTypeCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateActivityTypeCommandsHandler : IRequestHandler<UpdateActivityTypeCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateActivityTypeCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateActivityTypeCommands request, CancellationToken cancellationToken)
    {
        var activityType = _context.Get<Domain.Entities.ActivityType>().FirstOrDefault(x => x.Id == request.Id);

        if (activityType == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.ActivityType), request.Id);
        }

        _context.Get<Domain.Entities.ActivityType>().Update(activityType);
        await _context.SaveChangesAsync(cancellationToken);

        return activityType.Id;
    }
}
