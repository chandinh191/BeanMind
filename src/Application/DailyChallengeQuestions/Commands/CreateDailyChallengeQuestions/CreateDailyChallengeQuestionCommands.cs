using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.DailyChallengeQuestions.Commands.CreateDailyChallengeQuestions;
public class CreateDailyChallengeQuestionCommands : IRequest<Guid>
{
}

public class CreateDailyChallengeQuestionCommandsHandler : IRequestHandler<CreateDailyChallengeQuestionCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateDailyChallengeQuestionCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDailyChallengeQuestionCommands request, CancellationToken cancellationToken)
    {
        var dailyChallengeQuestion = new Domain.Entities.DailyChallengeQuestion()
        {

        };

        _context.Get<Domain.Entities.DailyChallengeQuestion>().Add(dailyChallengeQuestion);
        await _context.SaveChangesAsync(cancellationToken);
        return dailyChallengeQuestion.Id;
    }
}
