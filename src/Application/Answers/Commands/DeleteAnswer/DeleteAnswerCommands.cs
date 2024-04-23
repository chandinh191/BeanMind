using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Answers.Commands.DeleteAnswer;
public class DeleteAnswerCommands : IRequest<Guid>
{
    public Guid Id { get; set; }    
}

public class DeleteAnswerCommandsHandler : IRequestHandler<DeleteAnswerCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteAnswerCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteAnswerCommands request, CancellationToken cancellationToken)
    {
        var answer = await _context.Get<Domain.Entities.Answer>().FindAsync(request.Id);
        if (answer == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Answer), request.Id);
        }

        answer.IsDeleted = true;    
        _context.Get<Domain.Entities.Answer>().Update(answer);
        await _context.SaveChangesAsync(cancellationToken);
        return answer.Id;
    }
}
