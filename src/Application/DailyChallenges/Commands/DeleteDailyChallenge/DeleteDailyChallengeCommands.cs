using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.DailyChallenges.Commands.DeleteDailyChallenge;
public class DeleteDailyChallengeCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteDailyChallengeCommandHandler : IRequestHandler<DeleteDailyChallengeCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteDailyChallengeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteDailyChallengeCommands request, CancellationToken cancellationToken)
    {
        var dailyChallenge = await _context.Get<Domain.Entities.DailyChallenge>().FindAsync(request.Id);
        if (dailyChallenge == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.DailyChallenge), request.Id);
        }

        dailyChallenge.IsDeleted = true;    
        _context.Get<Domain.Entities.DailyChallenge>().Update(dailyChallenge);
        await _context.SaveChangesAsync(cancellationToken);
        return dailyChallenge.Id;
    }
}


