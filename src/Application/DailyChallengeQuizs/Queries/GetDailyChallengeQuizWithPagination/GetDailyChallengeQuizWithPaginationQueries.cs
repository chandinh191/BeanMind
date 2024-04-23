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

namespace BeanMind.Application.DailyChallengeQuizs.Queries.GetDailyChallengeQuizWithPagination;
public class GetDailyChallengeQuizWithPaginationQueries : IRequest<PaginatedList<DailyChallengeQuizModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }   
}

public class GetDailyChallengeQuizWithPaginationQueryHandler : IRequestHandler<GetDailyChallengeQuizWithPaginationQueries, PaginatedList<DailyChallengeQuizModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDailyChallengeQuizWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DailyChallengeQuizModel>> Handle(GetDailyChallengeQuizWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var dailyChallengeQuizs = _context.Get<Domain.Entities.DailyChallengeQuiz>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<DailyChallengeQuizModel>(dailyChallengeQuizs);

        var page = await PaginatedList<DailyChallengeQuizModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
