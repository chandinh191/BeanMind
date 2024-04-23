using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.DailyChallengeQuizs.Commands.UpdateDailyChallengeQuiz;
public class UpdateDailyChallengeQuizCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateDailyChallengeQuizCommandsHandler : IRequestHandler<UpdateDailyChallengeQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateDailyChallengeQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateDailyChallengeQuizCommands request, CancellationToken cancellationToken)
    {
        var dailyChallengeQuiz = await _context.Get<Domain.Entities.DailyChallengeQuiz>().FindAsync(request.Id);
        if (dailyChallengeQuiz == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.DailyChallengeQuiz), request.Id);
        }

        _context.Get<Domain.Entities.DailyChallengeQuiz>().Update(dailyChallengeQuiz);
        await _context.SaveChangesAsync(cancellationToken);
        return dailyChallengeQuiz.Id;
    }
}
