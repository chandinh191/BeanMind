using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.ChallengeLevels.Commands.CreateChallengeLevel;
public class CreateChallengeLevelCommands : IRequest<Guid>
{

}

public class CreateChallengeLevelCommandsHandler : IRequestHandler<CreateChallengeLevelCommands, Guid>
{
   private readonly IApplicationDbContext _context;

    public CreateChallengeLevelCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateChallengeLevelCommands request, CancellationToken cancellationToken)
    {
        var challengeLevel = new Domain.Entities.ChallengeLevel
        {
 
        };

        _context.Get<Domain.Entities.ChallengeLevel>().Add(challengeLevel);
        await _context.SaveChangesAsync(cancellationToken);
        return challengeLevel.Id;
    }
}
