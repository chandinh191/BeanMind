using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Interfaces;
using MediatR;

namespace BeanMind.Application.Videos.Commands.CreateVideo;
public class CreateVideoCommands : IRequest<Guid>
{

}

public class CreateVideoCommandsHandler : IRequestHandler<CreateVideoCommands, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateVideoCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateVideoCommands request, CancellationToken cancellationToken)
    {
        var video = new Domain.Entities.Video
        {

        };

        _context.Get<Domain.Entities.Video>().Add(video);
        await _context.SaveChangesAsync(cancellationToken);
        return video.Id;
    }
}


