using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Documents.Commands.UpdateDocument;
public class UpdateDocumentCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateDocumentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateDocumentCommands request, CancellationToken cancellationToken)
    {
        var document = await _context.Get<Domain.Entities.Document>().FindAsync(request.Id);
        if (document == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Document), request.Id);
        }

        _context.Get<Domain.Entities.Document>().Update(document);
        await _context.SaveChangesAsync(cancellationToken);
        return document.Id;
    }
}