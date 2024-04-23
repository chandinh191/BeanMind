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

namespace BeanMind.Application.Quizs.Queries.GetQuizWithPagination;
public class GetQuizWithPaginationQueries : IRequest<PaginatedList<QuizModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetQuizWithPaginationQueriesHandler : IRequestHandler<GetQuizWithPaginationQueries, PaginatedList<QuizModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuizWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<QuizModel>> Handle(GetQuizWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var quizs = _context.Get<Domain.Entities.Quiz>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<QuizModel>(quizs);

        var page = await PaginatedList<QuizModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
