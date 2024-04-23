using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.ChallengeLevels.Commands.DeleteChallengeLevel;
public class DeleteChallengeLevelCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteChallengeLevelCommandsHandler : IRequestHandler<DeleteChallengeLevelCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteChallengeLevelCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteChallengeLevelCommands request, CancellationToken cancellationToken)
    {
        var challengeLevel = await _context.Get<Domain.Entities.ChallengeLevel>().FindAsync(request.Id);
        if (challengeLevel == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.ChallengeLevel), request.Id);
        }

        challengeLevel.IsDeleted = true;
        _context.Get<Domain.Entities.ChallengeLevel>().Update(challengeLevel);
        await _context.SaveChangesAsync(cancellationToken);
        return challengeLevel.Id;
    }
}
