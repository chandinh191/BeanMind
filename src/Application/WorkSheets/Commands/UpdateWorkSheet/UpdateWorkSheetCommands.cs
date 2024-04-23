using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.WorkSheets.Commands.UpdateWorkSheet;
public class UpdateWorkSheetCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateWorkSheetCommandsHandler : IRequestHandler<UpdateWorkSheetCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateWorkSheetCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateWorkSheetCommands request, CancellationToken cancellationToken)
    {
        var workSheet = await _context.Get<Domain.Entities.Worksheet>().FindAsync(request.Id);
        if (workSheet == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Worksheet), request.Id);
        }

        _context.Get<Domain.Entities.Worksheet>().Update(workSheet);
        await _context.SaveChangesAsync(cancellationToken);

        return workSheet.Id;
    }
}
