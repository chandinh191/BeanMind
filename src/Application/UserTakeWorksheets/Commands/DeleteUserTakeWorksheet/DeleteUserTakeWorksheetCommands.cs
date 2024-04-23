using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.UserTakeWorksheets.Commands.DeleteUserTakeWorksheet;
public class DeleteUserTakeWorksheetCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteUserTakeWorksheetCommandsHandler : IRequestHandler<DeleteUserTakeWorksheetCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserTakeWorksheetCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteUserTakeWorksheetCommands request, CancellationToken cancellationToken)
    {
        var entity = await _context.Get<Domain.Entities.UserTakeWorksheet>().FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.UserTakeWorksheet), request.Id);
        }

        entity.IsDeleted = true;
        _context.Get<Domain.Entities.UserTakeWorksheet>().Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
