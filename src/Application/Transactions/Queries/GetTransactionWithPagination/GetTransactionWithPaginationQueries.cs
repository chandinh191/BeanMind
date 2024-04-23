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

namespace BeanMind.Application.Transactions.Queries.GetTransactionWithPagination;
public class GetTransactionWithPaginationQueries : IRequest<PaginatedList<TransactionModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }
}

public class GetTransactionWithPaginationQueriesHandler : IRequestHandler<GetTransactionWithPaginationQueries, PaginatedList<TransactionModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTransactionWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TransactionModel>> Handle(GetTransactionWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var transactions = _context.Get<Domain.Entities.Transaction>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        /*if (!string.IsNullOrEmpty(request.Search))
        {
            transactions = transactions.Where(x => x.Name.Contains(request.Search));
        }

        if (!string.IsNullOrEmpty(request.SortBy) && !string.IsNullOrEmpty(request.SortOrder))
        {
            transactions = transactions.OrderBy(request.SortBy + " " + request.SortOrder);
        }*/

        var map = _mapper.ProjectTo<TransactionModel>(transactions);

        var page = await PaginatedList<TransactionModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
