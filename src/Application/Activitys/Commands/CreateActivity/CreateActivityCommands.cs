using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Activitys.Commands.CreateActivity;
public class CreateActivityCommands : IRequest<Guid>
{

}
public class CreateActivityCommandsHandler : IRequestHandler<CreateActivityCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateActivityCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateActivityCommands request, CancellationToken cancellationToken)
    {
        var activity = new Domain.Entities.Activity()
        {
            
        };

        _context.Get<Domain.Entities.Activity>().Add(activity);
        await _context.SaveChangesAsync(cancellationToken);

        return activity.Id;
    }
}