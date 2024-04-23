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

namespace BeanMind.Application.Questions.Queries.GetQuestionWithPagination;
public class GetQuestionWithPaginationQueries : IRequest<PaginatedList<QuestionModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetQuestionWithPaginationQueriesHandler : IRequestHandler<GetQuestionWithPaginationQueries, PaginatedList<QuestionModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuestionWithPaginationQueriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<QuestionModel>> Handle(GetQuestionWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var questions = _context.Get<Domain.Entities.Question>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<QuestionModel>(questions);

        var page = await PaginatedList<QuestionModel>.CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
