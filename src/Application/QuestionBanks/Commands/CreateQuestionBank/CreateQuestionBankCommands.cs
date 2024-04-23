using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.QuestionBanks.Commands.CreateQuestionBank;
public class CreateQuestionBankCommands : IRequest<Guid>
{

}

public class CreateQuestionBankCommandsHandler : IRequestHandler<CreateQuestionBankCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateQuestionBankCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateQuestionBankCommands request, CancellationToken cancellationToken)
    {
        var questionBank = new Domain.Entities.QuestionBank()
        {

        };

        _context.Get<Domain.Entities.QuestionBank>().Add(questionBank);
        await _context.SaveChangesAsync(cancellationToken);
        return questionBank.Id;
    }
}
