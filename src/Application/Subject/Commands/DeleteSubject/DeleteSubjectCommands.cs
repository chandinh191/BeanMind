using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Subject.Commands.DeleteSubject;
public class DeleteSubjectCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteSubjectCommandsHandler : IRequestHandler<DeleteSubjectCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteSubjectCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteSubjectCommands request, CancellationToken cancellationToken)
    {
        var subject = _context.Get<Domain.Entities.Subject>().FirstOrDefault(x => x.Id == request.Id);

        if (subject == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Subject), request.Id);
        }

        subject.IsDeleted = true;
        _context.Get<Domain.Entities.Subject>().Update(subject);
        await _context.SaveChangesAsync(cancellationToken);

        return subject.Id;
    }
}

