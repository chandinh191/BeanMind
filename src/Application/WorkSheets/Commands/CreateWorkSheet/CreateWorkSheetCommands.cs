using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.WorkSheets.Commands.CreateWorkSheet;
public class CreateWorkSheetCommands : IRequest<Guid>
{

}

public class CreateWorkSheetCommandsHandler : IRequestHandler<CreateWorkSheetCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkSheetCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateWorkSheetCommands request, CancellationToken cancellationToken)
    {
        var workSheet = new Domain.Entities.Worksheet()
        {

        };

        _context.Get<Domain.Entities.Worksheet>().Add(workSheet);
        await _context.SaveChangesAsync(cancellationToken);
        return workSheet.Id;
    }
}
