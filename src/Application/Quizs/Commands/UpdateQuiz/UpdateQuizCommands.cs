using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Quizs.Commands.UpdateQuiz;
public class UpdateQuizCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateQuizCommandsHandler : IRequestHandler<UpdateQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateQuizCommands request, CancellationToken cancellationToken)
    {
        var quiz = await _context.Get<Domain.Entities.Quiz>().FindAsync(request.Id);
        if (quiz == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Quiz), request.Id);
        }

        _context.Get<Domain.Entities.Quiz>().Update(quiz);
        await _context.SaveChangesAsync(cancellationToken);

        return quiz.Id;
    }
}
