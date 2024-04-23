using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.DailyChallenges.Commands.CreateDailyChallenge;
public class CreateDailyChallengeCommands : IRequest<Guid>
{
}

public class CreateDailyChallengeCommandHandler : IRequestHandler<CreateDailyChallengeCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateDailyChallengeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDailyChallengeCommands request, CancellationToken cancellationToken)
    {
        var dailyChallenge = new Domain.Entities.DailyChallenge()
        {

        };

        _context.Get<Domain.Entities.DailyChallenge>().Add(dailyChallenge);
        await _context.SaveChangesAsync(cancellationToken);
        return dailyChallenge.Id;
    }
}
