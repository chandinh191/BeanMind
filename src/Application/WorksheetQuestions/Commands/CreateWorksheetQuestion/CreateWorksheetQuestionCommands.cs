using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.WorksheetQuestions.Commands.CreateWorksheetQuestion;
public class CreateWorksheetQuestionCommands : IRequest<Guid>
{

}

public class CreateWorksheetQuestionCommandsHandler : IRequestHandler<CreateWorksheetQuestionCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateWorksheetQuestionCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateWorksheetQuestionCommands request, CancellationToken cancellationToken)
    {
        var worksheetQuestion = new Domain.Entities.WorksheetQuestion
        {

        };

        _context.Get<Domain.Entities.WorksheetQuestion>().Add(worksheetQuestion);
        await _context.SaveChangesAsync(cancellationToken);
        return worksheetQuestion.Id;
    }
}
