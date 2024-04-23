using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.UserTakeDailyChallengeQuizs.Commands.DeleteUserTakeDailyChallengeQuiz;
public class DeleteUserTakeDailyChallengeQuizCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteUserTakeDailyChallengeQuizCommandsHandler : IRequestHandler<DeleteUserTakeDailyChallengeQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserTakeDailyChallengeQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteUserTakeDailyChallengeQuizCommands request, CancellationToken cancellationToken)
    {
        var entity = await _context.Get<Domain.Entities.UserTakeDailyChallengeQuiz>().FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.UserTakeDailyChallengeQuiz), request.Id);
        }

        entity.IsDeleted = true;
        _context.Get<Domain.Entities.UserTakeDailyChallengeQuiz>().Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
