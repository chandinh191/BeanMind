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

namespace BeanMind.Application.WorksheetQuestions.Queries.GetWorksheetQuestionWithPagination;
public class GetWorksheetQuestionWithPaginationQueries : IRequest<PaginatedList<WorksheetQuestionModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }
    public Guid WorksheetId { get; set; }
}

public class GetWorksheetQuestionWithPaginationQueriesHandler : IRequestHandler<GetWorksheetQuestionWithPaginationQueries, PaginatedList<WorksheetQuestionModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorksheetQuestionWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WorksheetQuestionModel>> Handle(GetWorksheetQuestionWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var worksheetQuestions = _context.Get<Domain.Entities.WorksheetQuestion>()
            .Where(x => x.WorksheetId == request.WorksheetId && x.IsDeleted == false).AsNoTracking();

        /*if (!string.IsNullOrEmpty(request.Search))
        {
            worksheetQuestions = worksheetQuestions.Where(x => x.Question.Question.Contains(request.Search));
        }

        if (!string.IsNullOrEmpty(request.SortBy) && !string.IsNullOrEmpty(request.SortOrder))
        {
            worksheetQuestions = worksheetQuestions.OrderBy($"{request.SortBy} {request.SortOrder}");
        }*/

        var map = _mapper.ProjectTo<WorksheetQuestionModel>(worksheetQuestions);

        var page = await PaginatedList<WorksheetQuestionModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
