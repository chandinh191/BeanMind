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

namespace BeanMind.Application.Activitys.Queries.GetActivityWithPagination;
public class GetActivityWithPaginationQueries : IRequest<PaginatedList<ActivityModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetActivityWithPaginationQueriesHandler : IRequestHandler<GetActivityWithPaginationQueries, PaginatedList<ActivityModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetActivityWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ActivityModel>> Handle(GetActivityWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var comment = _context.Get<Domain.Entities.Activity>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<ActivityModel>(comment);

        var page = await PaginatedList<ActivityModel>
            .CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}