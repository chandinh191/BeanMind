using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Transactions.Commands.CreateTransaction;
public class CreateTransactionCommands : IRequest<Guid>
{

}

public class CreateTransactionCommandsHandler : IRequestHandler<CreateTransactionCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTransactionCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateTransactionCommands request, CancellationToken cancellationToken)
    {
        var transaction = new Domain.Entities.Transaction
        {

        };

        _context.Get<Domain.Entities.Transaction>().Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction.Id;
    }
}
