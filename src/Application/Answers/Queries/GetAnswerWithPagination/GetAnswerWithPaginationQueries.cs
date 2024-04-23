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

namespace BeanMind.Application.Answers.Queries.GetAnswerWithPagination;
public class GetAnswerWithPaginationQueries : IRequest<PaginatedList<AnswerModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetAnswerWithPaginationQueriesHandler : IRequestHandler<GetAnswerWithPaginationQueries, PaginatedList<AnswerModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAnswerWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AnswerModel>> Handle(GetAnswerWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var answer = _context.Get<Domain.Entities.Answer>()
             .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<AnswerModel>(answer);

        var page = await PaginatedList<AnswerModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}

