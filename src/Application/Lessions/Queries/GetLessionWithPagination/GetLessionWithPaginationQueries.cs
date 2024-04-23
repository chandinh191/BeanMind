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

namespace BeanMind.Application.Lessions.Queries.GetLessionWithPagination;
public class GetLessionWithPaginationQueries : IRequest<PaginatedList<LessionModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetLessionWithPaginationQueryHandler : IRequestHandler<GetLessionWithPaginationQueries, PaginatedList<LessionModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLessionWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LessionModel>> Handle(GetLessionWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var lessions = _context.Get<Domain.Entities.Lession>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<LessionModel>(lessions);
      
        var page = await PaginatedList<LessionModel>.CreateAsync(map, request.PageNumber, request.PageSize);
        
        return page;
    }
}
