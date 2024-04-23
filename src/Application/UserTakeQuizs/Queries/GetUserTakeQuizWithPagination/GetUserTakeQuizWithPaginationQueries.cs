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

namespace BeanMind.Application.UserTakeQuizs.Queries.GetUserTakeQuizWithPagination;
public class GetUserTakeQuizWithPaginationQueries : IRequest<PaginatedList<UserTakeQuizModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetUserTakeQuizWithPaginationQueriesHandler : IRequestHandler<GetUserTakeQuizWithPaginationQueries, PaginatedList<UserTakeQuizModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserTakeQuizWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserTakeQuizModel>> Handle(GetUserTakeQuizWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var userTakeQuizs = _context.Get<Domain.Entities.UserTakeQuiz>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<UserTakeQuizModel>(userTakeQuizs);

        var page = await PaginatedList<UserTakeQuizModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
