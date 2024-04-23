using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.WorksheetQuestions.Commands.UpdateWorksheetQuestion;
public class UpdateWorksheetQuestionCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateWorksheetQuestionCommandsHandler : IRequestHandler<UpdateWorksheetQuestionCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateWorksheetQuestionCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateWorksheetQuestionCommands request, CancellationToken cancellationToken)
    {
        var worksheetQuestion = await _context.Get<Domain.Entities.WorksheetQuestion>().FindAsync(request.Id);
        if (worksheetQuestion == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.WorksheetQuestion), request.Id);
        }

        _context.Get<Domain.Entities.WorksheetQuestion>().Update(worksheetQuestion);
        await _context.SaveChangesAsync(cancellationToken);

        return worksheetQuestion.Id;
    }
}
