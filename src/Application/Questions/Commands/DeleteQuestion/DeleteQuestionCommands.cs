using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Questions.Commands.DeleteQuestion;
public class DeleteQuestionCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteQuestionCommandsHandler : IRequestHandler<DeleteQuestionCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteQuestionCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteQuestionCommands request, CancellationToken cancellationToken)
    {
        var question = await _context.Get<Domain.Entities.Question>().FindAsync(request.Id);
        if (question == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Question), request.Id);
        }

        question.IsDeleted = true;
        _context.Get<Domain.Entities.Question>().Update(question);
        await _context.SaveChangesAsync(cancellationToken);

        return question.Id;
    }
}
