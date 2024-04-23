using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Quizs.Commands.CreateQuiz;
public class CreateQuizCommands : IRequest<Guid>
{
}

public class CreateQuizCommandsHandler : IRequestHandler<CreateQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateQuizCommands request, CancellationToken cancellationToken)
    {
        var quiz = new Domain.Entities.Quiz
        {

        };

        _context.Get<Domain.Entities.Quiz>().Add(quiz);
        await _context.SaveChangesAsync(cancellationToken);
        return quiz.Id;
    }
}
