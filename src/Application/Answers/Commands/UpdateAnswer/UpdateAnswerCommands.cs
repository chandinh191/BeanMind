using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Answers.Commands.UpdateAnswer;
public class UpdateAnswerCommands : IRequest<Guid>
{
    public Guid Id { get; set; }    
}

public class UpdateAnswerCommandsHandler : IRequestHandler<UpdateAnswerCommands, Guid>
{
    private readonly IApplicationDbContext _context;    

    public UpdateAnswerCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateAnswerCommands request, CancellationToken cancellationToken)
    {
        var answer = await _context.Get<Domain.Entities.Answer>().FindAsync(request.Id);
        if (answer == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Answer), request.Id);
        }

        _context.Get<Domain.Entities.Answer>().Update(answer);
        await _context.SaveChangesAsync(cancellationToken);
        return answer.Id;
    }
}
