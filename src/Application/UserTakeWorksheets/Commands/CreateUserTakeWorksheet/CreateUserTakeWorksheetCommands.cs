using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.UserTakeWorksheets.Commands.CreateUserTakeWorksheet;
public class CreateUserTakeWorksheetCommands : IRequest<Guid>
{ 

}

public class CreateUserTakeWorksheetCommandsHandler : IRequestHandler<CreateUserTakeWorksheetCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateUserTakeWorksheetCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUserTakeWorksheetCommands request, CancellationToken cancellationToken)
    {
        var userTakeWorksheet = new Domain.Entities.UserTakeWorksheet
        {
            
        };

        _context.Get<Domain.Entities.UserTakeWorksheet>().Add(userTakeWorksheet);
        await _context.SaveChangesAsync(cancellationToken);
        return userTakeWorksheet.Id;
    }
}
