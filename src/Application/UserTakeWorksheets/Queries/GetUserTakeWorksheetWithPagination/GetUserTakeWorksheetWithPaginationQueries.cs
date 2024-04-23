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

namespace BeanMind.Application.UserTakeWorksheets.Queries.GetUserTakeWorksheetWithPagination;
public class GetUserTakeWorksheetWithPaginationQueries : IRequest<PaginatedList<UserTakeWorksheetModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }
    public string UserId { get; set; }
    public string WorksheetId { get; set; }
}

public class GetUserTakeWorksheetWithPaginationQueriesHandler : IRequestHandler<GetUserTakeWorksheetWithPaginationQueries, PaginatedList<UserTakeWorksheetModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserTakeWorksheetWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserTakeWorksheetModel>> Handle(GetUserTakeWorksheetWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var userTakeWorksheets = _context.Get<Domain.Entities.UserTakeWorksheet>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        /*if (!string.IsNullOrEmpty(request.UserId))
        {
            userTakeWorksheets = userTakeWorksheets.Where(x => x.UserId == request.UserId);
        }

        if (!string.IsNullOrEmpty(request.WorksheetId))
        {
            userTakeWorksheets = userTakeWorksheets.Where(x => x.WorksheetId == request.WorksheetId);
        }

        if (!string.IsNullOrEmpty(request.Search))
        {
            userTakeWorksheets = userTakeWorksheets.Where(x => x.UserId.Contains(request.Search) || x.WorksheetId.Contains(request.Search));
        }

        if (!string.IsNullOrEmpty(request.SortBy) && !string.IsNullOrEmpty(request.SortOrder))
        {
            switch (request.SortBy)
            {
                case "UserId":
                    userTakeWorksheets = request.SortOrder == "asc" ? userTakeWorksheets.OrderBy(x => x.UserId) : userTakeWorksheets.OrderByDescending(x => x.UserId);
                    break;
                case "WorksheetId":
                    userTakeWorksheets = request.SortOrder == "asc" ? userTakeWorksheets.OrderBy(x => x.WorksheetId) : userTakeWorksheets.OrderByDescending(x => x.WorksheetId);
                    break;
            }
        }*/

        var map = _mapper.ProjectTo<UserTakeWorksheetModel>(userTakeWorksheets);

        var page = await PaginatedList<UserTakeWorksheetModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
