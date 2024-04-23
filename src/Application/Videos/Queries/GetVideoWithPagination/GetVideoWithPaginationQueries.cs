using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BeanMind.Application.Common.Interfaces;
using BeanMind.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeanMind.Application.Videos.Queries.GetVideoWithPagination;
public class GetVideoWithPaginationQueries : IRequest<PaginatedList<VideoModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }
}

public class GetVideoWithPaginationQueriesHandler : IRequestHandler<GetVideoWithPaginationQueries, PaginatedList<VideoModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVideoWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<VideoModel>> Handle(GetVideoWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var videos = _context.Get<Domain.Entities.Video>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        /*if (!string.IsNullOrEmpty(request.Search))
        {
            videos = videos.Where(x => x.Title.Contains(request.Search));
        }

        if (!string.IsNullOrEmpty(request.SortBy) && !string.IsNullOrEmpty(request.SortOrder))
        {
            videos = videos.OrderBy($"{request.SortBy} {request.SortOrder}");
        }*/

        var map = _mapper.ProjectTo<VideoModel>(videos);

        var page = await PaginatedList<VideoModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
