using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.ActivityTypes.Commands.CreateActivityType;
public class CreateActivityTypeCommands : IRequest<Guid>
{

}

public class CreateActivityTypeCommandsHandler : IRequestHandler<CreateActivityTypeCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateActivityTypeCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateActivityTypeCommands request, CancellationToken cancellationToken)
    {
        var activityType = new Domain.Entities.ActivityType()
        {
            
        };

        _context.Get<Domain.Entities.ActivityType>().Add(activityType);
        await _context.SaveChangesAsync(cancellationToken);

        return activityType.Id;
    }   
}
