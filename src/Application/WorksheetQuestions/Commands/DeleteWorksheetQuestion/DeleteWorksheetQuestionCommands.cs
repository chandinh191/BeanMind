using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.WorksheetQuestions.Commands.DeleteWorksheetQuestion;
public class DeleteWorksheetQuestionCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteWorksheetQuestionCommandsHandler : IRequestHandler<DeleteWorksheetQuestionCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteWorksheetQuestionCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteWorksheetQuestionCommands request, CancellationToken cancellationToken)
    {
        var entity = await _context.Get<Domain.Entities.WorksheetQuestion>().FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.WorksheetQuestion), request.Id);
        }

        entity.IsDeleted = true;
        _context.Get<Domain.Entities.WorksheetQuestion>().Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
