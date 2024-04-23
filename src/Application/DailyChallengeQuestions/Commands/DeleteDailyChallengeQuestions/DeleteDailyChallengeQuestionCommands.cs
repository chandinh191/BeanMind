using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.DailyChallengeQuestions.Commands.DeleteDailyChallengeQuestions;
public class DeleteDailyChallengeQuestionCommands : IRequest<Guid>
{
    public Guid Id { get; set; }    
}

public class DeleteDailyChallengeQuestionCommandsHandler : IRequestHandler<DeleteDailyChallengeQuestionCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteDailyChallengeQuestionCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteDailyChallengeQuestionCommands request, CancellationToken cancellationToken)
    {
        var dailyChallengeQuestion = await _context.Get<Domain.Entities.DailyChallengeQuestion>().FindAsync(request.Id);
        if (dailyChallengeQuestion == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.DailyChallengeQuestion), request.Id);
        }

        dailyChallengeQuestion.IsDeleted = true;    
        _context.Get<Domain.Entities.DailyChallengeQuestion>().Update(dailyChallengeQuestion);
        await _context.SaveChangesAsync(cancellationToken);
        return dailyChallengeQuestion.Id;
    }
}
