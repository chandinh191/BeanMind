using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.UserTakeWorksheets.Commands.UpdateUserTakeWorksheet;
public class UpdateUserTakeWorksheetCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateUserTakeWorksheetCommandsHandler : IRequestHandler<UpdateUserTakeWorksheetCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserTakeWorksheetCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateUserTakeWorksheetCommands request, CancellationToken cancellationToken)
    {
        var userTakeWorksheet = await _context.Get<Domain.Entities.UserTakeWorksheet>().FindAsync(request.Id);
        if (userTakeWorksheet == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.UserTakeWorksheet), request.Id);
        }

        _context.Get<Domain.Entities.UserTakeWorksheet>().Update(userTakeWorksheet);
        await _context.SaveChangesAsync(cancellationToken);

        return userTakeWorksheet.Id;
    }
}
