using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.UserTakeQuizs.Commands.CreateUserTakeQuiz;
public class CreateUserTakeQuizCommands : IRequest<Guid>
{

}

public class CreateUserTakeQuizCommandsHandler : IRequestHandler<CreateUserTakeQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateUserTakeQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUserTakeQuizCommands request, CancellationToken cancellationToken)
    {
        var userTakeQuiz = new Domain.Entities.UserTakeQuiz
        {

        };

        _context.Get<Domain.Entities.UserTakeQuiz>().Add(userTakeQuiz);
        await _context.SaveChangesAsync(cancellationToken);
        return userTakeQuiz.Id;
    }
}

