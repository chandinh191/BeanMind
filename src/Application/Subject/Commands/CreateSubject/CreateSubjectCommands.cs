using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using BeanMind.Application.Common.Models;
using MediatR;

namespace BeanMind.Application.Subject.Commands.CreateSubject;
public class CreateSubjectCommands : IRequest<Guid>
{

}

public class CreateSubjectCommandsHandler : IRequestHandler<CreateSubjectCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSubjectCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSubjectCommands request, CancellationToken cancellationToken)
    {
        var subject = new Domain.Entities.Subject()
        {
            
        };

        _context.Get<Domain.Entities.Subject>().Add(subject);
        await _context.SaveChangesAsync(cancellationToken);

        return subject.Id;
    }
}
