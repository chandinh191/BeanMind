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

namespace BeanMind.Application.WorkSheets.Queries.GetWorkSheetWithPagination;
public class GetWorkSheetWithPaginationQueries : IRequest<PaginatedList<WorksheetModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }
}

public class GetWorkSheetWithPaginationQueriesHandler : IRequestHandler<GetWorkSheetWithPaginationQueries, PaginatedList<WorksheetModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkSheetWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WorksheetModel>> Handle(GetWorkSheetWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var worksheets = _context.Get<Domain.Entities.Worksheet>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        /*if (!string.IsNullOrEmpty(request.Search))
        {
            worksheets = worksheets.Where(x => x.Name.Contains(request.Search));
        }

        if (!string.IsNullOrEmpty(request.SortBy) && !string.IsNullOrEmpty(request.SortOrder))
        {
            if (request.SortOrder == "asc")
            {
                worksheets = worksheets.OrderBy(x => x.GetType().GetProperty(request.SortBy).GetValue(x, null));
            }
            else
            {
                worksheets = worksheets.OrderByDescending(x => x.GetType().GetProperty(request.SortBy).GetValue(x, null));
            }
        }*/

        var map = _mapper.ProjectTo<WorksheetModel>(worksheets);

        var page = await PaginatedList<WorksheetModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
