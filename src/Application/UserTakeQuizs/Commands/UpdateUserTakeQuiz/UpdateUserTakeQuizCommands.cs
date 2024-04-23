using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.UserTakeQuizs.Commands.UpdateUserTakeQuiz;
public class UpdateUserTakeQuizCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateUserTakeQuizCommandsHandler : IRequestHandler<UpdateUserTakeQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserTakeQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateUserTakeQuizCommands request, CancellationToken cancellationToken)
    {
        var userTakeQuiz = await _context.Get<Domain.Entities.UserTakeQuiz>().FindAsync(request.Id);
        if (userTakeQuiz == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.UserTakeQuiz), request.Id);
        }

        _context.Get<Domain.Entities.UserTakeQuiz>().Update(userTakeQuiz);
        await _context.SaveChangesAsync(cancellationToken);

        return userTakeQuiz.Id;
    }
}
