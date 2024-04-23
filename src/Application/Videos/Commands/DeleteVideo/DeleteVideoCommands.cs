using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Exceptions;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Videos.Commands.DeleteVideo;
public class DeleteVideoCommands : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class DeleteVideoCommandsHandler : IRequestHandler<DeleteVideoCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public DeleteVideoCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteVideoCommands request, CancellationToken cancellationToken)
    {
        var video = await _context.Get<Domain.Entities.Video>().FindAsync(request.Id);
        if (video == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Video), request.Id);
        }

        video.IsDeleted = true;
        _context.Get<Domain.Entities.Video>().Update(video);
        await _context.SaveChangesAsync(cancellationToken);

        return video.Id;
    }
}
