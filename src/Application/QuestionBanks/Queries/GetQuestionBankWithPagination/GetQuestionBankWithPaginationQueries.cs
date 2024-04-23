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

namespace BeanMind.Application.QuestionBanks.Queries.GetQuestionBankWithPagination;
public class GetQuestionBankWithPaginationQueries : IRequest<PaginatedList<QuestionBankModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }   
}

public class GetQuestionBankWithPaginationQueryHandler : IRequestHandler<GetQuestionBankWithPaginationQueries, PaginatedList<QuestionBankModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuestionBankWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<QuestionBankModel>> Handle(GetQuestionBankWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var questionBanks = _context.Get<Domain.Entities.QuestionBank>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<QuestionBankModel>(questionBanks);
      
        var page = await PaginatedList<QuestionBankModel>.CreateAsync(map, request.PageNumber, request.PageSize);
        
        return page;
    }
}
