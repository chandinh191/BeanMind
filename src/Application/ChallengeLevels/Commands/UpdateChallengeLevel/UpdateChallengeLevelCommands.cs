using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.ChallengeLevels.Commands.UpdateChallengeLevel;
public class UpdateChallengeLevelCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateChallengeLevelCommandsHandler : IRequestHandler<UpdateChallengeLevelCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateChallengeLevelCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateChallengeLevelCommands request, CancellationToken cancellationToken)
    {
        var challengeLevel = _context.Get<Domain.Entities.ChallengeLevel>().Find(request.Id);
        if (challengeLevel == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.ChallengeLevel), request.Id);
        }

        _context.Get<Domain.Entities.ChallengeLevel>().Update(challengeLevel);
        await _context.SaveChangesAsync(cancellationToken);
        return challengeLevel.Id;
    }
}


