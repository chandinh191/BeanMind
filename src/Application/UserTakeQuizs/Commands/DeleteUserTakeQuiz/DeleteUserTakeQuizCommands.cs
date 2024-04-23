using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.UserTakeQuizs.Commands.DeleteUserTakeQuiz;
public class DeleteUserTakeQuizCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteUserTakeQuizCommandsHandler : IRequestHandler<DeleteUserTakeQuizCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserTakeQuizCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteUserTakeQuizCommands request, CancellationToken cancellationToken)
    {
        var entity = await _context.Get<Domain.Entities.UserTakeQuiz>().FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.UserTakeQuiz), request.Id);
        }

        entity.IsDeleted = true;
        _context.Get<Domain.Entities.UserTakeQuiz>().Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
