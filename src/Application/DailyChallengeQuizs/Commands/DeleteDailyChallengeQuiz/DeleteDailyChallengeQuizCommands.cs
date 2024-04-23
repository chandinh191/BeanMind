using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.DailyChallengeQuizs.Commands.DeleteDailyChallengeQuiz;
public class DeleteDailyChallengeQuizCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteDailyChallengeQuizCommandsHandler : IRequestHandler<DeleteDailyChallengeQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteDailyChallengeQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteDailyChallengeQuizCommands request, CancellationToken cancellationToken)
    {
        var dailyChallengeQuiz = await _context.Get<Domain.Entities.DailyChallengeQuiz>().FindAsync(request.Id);
        if (dailyChallengeQuiz == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.DailyChallengeQuiz), request.Id);
        }

        dailyChallengeQuiz.IsDeleted = true;
        _context.Get<Domain.Entities.DailyChallengeQuiz>().Update(dailyChallengeQuiz);
        await _context.SaveChangesAsync(cancellationToken);
        return dailyChallengeQuiz.Id;
    }
}
