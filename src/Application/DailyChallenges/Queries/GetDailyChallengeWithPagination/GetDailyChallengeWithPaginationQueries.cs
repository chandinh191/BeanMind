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

namespace BeanMind.Application.DailyChallenges.Queries.GetDailyChallengeWithPagination;
public class GetDailyChallengeWithPaginationQueries : IRequest<PaginatedList<DailyChallengedModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetDailyChallengeWithPaginationQueryHandler : IRequestHandler<GetDailyChallengeWithPaginationQueries, PaginatedList<DailyChallengedModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDailyChallengeWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DailyChallengedModel>> Handle(GetDailyChallengeWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var dailyChallenges = _context.Get<Domain.Entities.DailyChallenge>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<DailyChallengedModel>(dailyChallenges);
      
        var page = await PaginatedList<DailyChallengedModel>.CreateAsync(map, request.PageNumber, request.PageSize);
        
        return page;
    }
}
