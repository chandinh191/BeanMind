using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.UserTakeDailyChallengeQuizs.Commands.UpdateUserTakeDailyChallengeQuiz;
public class UpdateUserTakeDailyChallengeQuizCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateUserTakeDailyChallengeQuizCommandsHandler : IRequestHandler<UpdateUserTakeDailyChallengeQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserTakeDailyChallengeQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateUserTakeDailyChallengeQuizCommands request, CancellationToken cancellationToken)
    {
        var userTakeDailyChallengeQuiz = await _context.Get<Domain.Entities.UserTakeDailyChallengeQuiz>().FindAsync(request.Id);
        if (userTakeDailyChallengeQuiz == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.UserTakeDailyChallengeQuiz), request.Id);
        }

        _context.Get<Domain.Entities.UserTakeDailyChallengeQuiz>().Update(userTakeDailyChallengeQuiz);
        await _context.SaveChangesAsync(cancellationToken);

        return userTakeDailyChallengeQuiz.Id;
    }
}
