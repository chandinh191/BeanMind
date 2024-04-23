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

namespace BeanMind.Application.DailyChallengeQuestions.Queries.GetDailyChallengeQuestionsWithPagination;
public class GetDailyChallengeQuestionsWithPaginationQueries : IRequest<PaginatedList<DailyChallengeQuestionModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }
}

public class GetDailyChallengeQuestionsWithPaginationQueryHandler : IRequestHandler<GetDailyChallengeQuestionsWithPaginationQueries, PaginatedList<DailyChallengeQuestionModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDailyChallengeQuestionsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DailyChallengeQuestionModel>> Handle(GetDailyChallengeQuestionsWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var dailyChallengeQuestions = _context.Get<Domain.Entities.DailyChallengeQuestion>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<DailyChallengeQuestionModel>(dailyChallengeQuestions);

        var page = await PaginatedList<DailyChallengeQuestionModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
