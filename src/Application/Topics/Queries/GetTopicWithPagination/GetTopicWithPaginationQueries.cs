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

namespace BeanMind.Application.Topics.Queries.GetTopicWithPagination;
public class GetTopicWithPaginationQueries : IRequest<PaginatedList<TopicModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }
}

public class GetTopicWithPaginationQueriesHandler : IRequestHandler<GetTopicWithPaginationQueries, PaginatedList<TopicModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTopicWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TopicModel>> Handle(GetTopicWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var topics = _context.Get<Domain.Entities.Topic>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        /*if (!string.IsNullOrEmpty(request.Search))
        {
            topics = topics.Where(x => x.Name.Contains(request.Search));
        }

        if (!string.IsNullOrEmpty(request.SortBy) && !string.IsNullOrEmpty(request.SortOrder))
        {
            topics = topics.OrderBy($"{request.SortBy} {request.SortOrder}");
        }*/

        var map = _mapper.ProjectTo<TopicModel>(topics);

        var page = await PaginatedList<TopicModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
