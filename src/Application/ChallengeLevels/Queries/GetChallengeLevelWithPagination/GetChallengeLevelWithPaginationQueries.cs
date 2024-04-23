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

namespace BeanMind.Application.ChallengeLevels.Queries.GetChallengeLevelWithPagination;
public class GetChallengeLevelWithPaginationQueries : IRequest<PaginatedList<ChallengeLevelModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetChallengeLevelWithPaginationQueriesHandler : IRequestHandler<GetChallengeLevelWithPaginationQueries, PaginatedList<ChallengeLevelModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetChallengeLevelWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ChallengeLevelModel>> Handle(GetChallengeLevelWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var challengeLevels = _context.Get<Domain.Entities.ChallengeLevel>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<ChallengeLevelModel>(challengeLevels);

        var page = await PaginatedList<ChallengeLevelModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}


