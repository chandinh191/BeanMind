using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.QuestionBanks.Commands.DeleteQuestionBank;
public class DeleteQuestionBankCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteQuestionBankCommandsHandler : IRequestHandler<DeleteQuestionBankCommands, Guid>
{
  private readonly IApplicationDbContext _context;

    public DeleteQuestionBankCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteQuestionBankCommands request, CancellationToken cancellationToken)
    {
        var questionBank = await _context.Get<Domain.Entities.QuestionBank>().FindAsync(request.Id);
        if (questionBank == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.QuestionBank), request.Id);
        }

        questionBank.IsDeleted = true;
        _context.Get<Domain.Entities.QuestionBank>().Update(questionBank);
        await _context.SaveChangesAsync(cancellationToken);
        return questionBank.Id;
    }
}
