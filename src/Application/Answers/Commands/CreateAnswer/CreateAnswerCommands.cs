using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Answers.Commands.CreateAnswer;
public class CreateAnswerCommands : IRequest<Guid>
{

}

public class CreateAnswerHandler : IRequestHandler<CreateAnswerCommands, Guid>
{
    private readonly IApplicationDbContext _context;    

    public CreateAnswerHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAnswerCommands request, CancellationToken cancellationToken)
    {
        var answer = new Domain.Entities.Answer
        {
           
        };

        _context.Get<Domain.Entities.Answer>().Add(answer);
        await _context.SaveChangesAsync(cancellationToken);
        return answer.Id;
    }
}
