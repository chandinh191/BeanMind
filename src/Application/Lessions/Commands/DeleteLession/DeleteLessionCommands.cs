using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Lessions.Commands.DeleteLession;
public class DeleteLessionCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteLessionCommandsHandler : IRequestHandler<DeleteLessionCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteLessionCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteLessionCommands request, CancellationToken cancellationToken)
    {
        var lession = await _context.Get<Domain.Entities.Lession>().FindAsync(request.Id);

        if (lession == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Lession), request.Id);
        }

        lession.IsDeleted = true;
        _context.Get<Domain.Entities.Lession>().Update(lession);

        await _context.SaveChangesAsync(cancellationToken);

        return lession.Id;
    }
}
