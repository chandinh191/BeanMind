using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.UserTakeDailyChallengeQuizs.Commands.CreateUserTakeDailyChallengeQuiz;
public class CreateUserTakeDailyChallengeQuizCommands : IRequest<Guid>
{

}

public class CreateUserTakeDailyChallengeQuizCommandsHandler : IRequestHandler<CreateUserTakeDailyChallengeQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateUserTakeDailyChallengeQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUserTakeDailyChallengeQuizCommands request, CancellationToken cancellationToken)
    {
        var userTakeDailyChallengeQuiz = new Domain.Entities.UserTakeDailyChallengeQuiz
        {

        };

        _context.Get<Domain.Entities.UserTakeDailyChallengeQuiz>().Add(userTakeDailyChallengeQuiz);
        await _context.SaveChangesAsync(cancellationToken);
        return userTakeDailyChallengeQuiz.Id;
    }
}
