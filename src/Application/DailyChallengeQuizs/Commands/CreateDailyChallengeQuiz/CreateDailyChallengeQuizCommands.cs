using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.DailyChallengeQuizs.Commands.CreateDailyChallengeQuiz;
public class CreateDailyChallengeQuizCommands : IRequest<Guid>
{

}

public class CreateDailyChallengeQuizCommandsHandler : IRequestHandler<CreateDailyChallengeQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateDailyChallengeQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDailyChallengeQuizCommands request, CancellationToken cancellationToken)
    {
        var dailyChallengeQuiz = new Domain.Entities.DailyChallengeQuiz()
        {

        };

        _context.Get<Domain.Entities.DailyChallengeQuiz>().Add(dailyChallengeQuiz);
        await _context.SaveChangesAsync(cancellationToken);
        return dailyChallengeQuiz.Id;
    }
}
