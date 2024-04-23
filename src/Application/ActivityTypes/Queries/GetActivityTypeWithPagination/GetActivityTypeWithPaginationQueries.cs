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

namespace BeanMind.Application.ActivityTypes.Queries.GetActivityTypeWithPagination;
public class GetActivityTypeWithPaginationQueries : IRequest<PaginatedList<ActivityTypeModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetActivityTypeWithPaginationQueriesHandler : IRequestHandler<GetActivityTypeWithPaginationQueries, PaginatedList<ActivityTypeModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetActivityTypeWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ActivityTypeModel>> Handle(GetActivityTypeWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var comment = _context.Get<Domain.Entities.ActivityType>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<ActivityTypeModel>(comment);

        var page = await PaginatedList<ActivityTypeModel>
            .CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
