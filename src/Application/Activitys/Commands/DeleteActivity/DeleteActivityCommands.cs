using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Activitys.Commands.DeleteActivity;
public class DeleteActivityCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteActivityCommandsHandler : IRequestHandler<DeleteActivityCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteActivityCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteActivityCommands request, CancellationToken cancellationToken)
    {
        var activity = _context.Get<Domain.Entities.Activity>().FirstOrDefault(x => x.Id == request.Id);

        if (activity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Activity), request.Id);
        }

        activity.IsDeleted = true;
        _context.Get<Domain.Entities.Activity>().Update(activity);
        await _context.SaveChangesAsync(cancellationToken);

        return activity.Id;
    }
}

