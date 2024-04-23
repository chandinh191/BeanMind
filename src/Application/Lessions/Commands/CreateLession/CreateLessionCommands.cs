using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Lessions.Commands.CreateLession;
public class CreateLessionCommands : IRequest<Guid>
{
}

public class CreateLessionCommandsHandler : IRequestHandler<CreateLessionCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateLessionCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateLessionCommands request, CancellationToken cancellationToken)
    {
        var lession = new Domain.Entities.Lession()
        {

        };

        _context.Get<Domain.Entities.Lession>().Add(lession);
        await _context.SaveChangesAsync(cancellationToken);
        return lession.Id;
    }
}
