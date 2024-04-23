using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Documents.Commands.CreateDocument;
public class CreateDocumentCommands : IRequest<Guid>
{

}

public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateDocumentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDocumentCommands request, CancellationToken cancellationToken)
    {
        var document = new Domain.Entities.Document()
        {

        };

        _context.Get<Domain.Entities.Document>().Add(document);
        await _context.SaveChangesAsync(cancellationToken);
        return document.Id;
    }
}
