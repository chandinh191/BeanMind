using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.WorkSheets.Commands.DeleteWorkSheet;
public class DeleteWorkSheetCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteWorkSheetCommandsHandler : IRequestHandler<DeleteWorkSheetCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteWorkSheetCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteWorkSheetCommands request, CancellationToken cancellationToken)
    {
        var entity = await _context.Get<Domain.Entities.Worksheet>().FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Worksheet), request.Id);
        }

        entity.IsDeleted = true;
        _context.Get<Domain.Entities.Worksheet>().Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
