using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Questions.Commands.CreateQuestion;
public class CreateQuestionCommands : IRequest<Guid>
{

}

public class CreateQuestionCommandsHandler : IRequestHandler<CreateQuestionCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateQuestionCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateQuestionCommands request, CancellationToken cancellationToken)
    {
        var question = new Domain.Entities.Question 
        {

        };

        _context.Get<Domain.Entities.Question>().Add(question);
        await _context.SaveChangesAsync(cancellationToken);
        return question.Id;
    }
}
