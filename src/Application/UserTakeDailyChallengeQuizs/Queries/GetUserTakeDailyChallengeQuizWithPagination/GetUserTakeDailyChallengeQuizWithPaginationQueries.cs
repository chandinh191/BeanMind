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

namespace BeanMind.Application.UserTakeDailyChallengeQuizs.Queries.GetUserTakeDailyChallengeQuizWithPagination;
public class GetUserTakeDailyChallengeQuizWithPaginationQueries : IRequest<PaginatedList<UserTakeDailyChallengeQuizModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetUserTakeDailyChallengeQuizWithPaginationQueriesHandler : IRequestHandler<GetUserTakeDailyChallengeQuizWithPaginationQueries, PaginatedList<UserTakeDailyChallengeQuizModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserTakeDailyChallengeQuizWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserTakeDailyChallengeQuizModel>> Handle(GetUserTakeDailyChallengeQuizWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var userTakeDailyChallengeQuizs = _context.Get<Domain.Entities.UserTakeDailyChallengeQuiz>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<UserTakeDailyChallengeQuizModel>(userTakeDailyChallengeQuizs);

        var page = await PaginatedList<UserTakeDailyChallengeQuizModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
