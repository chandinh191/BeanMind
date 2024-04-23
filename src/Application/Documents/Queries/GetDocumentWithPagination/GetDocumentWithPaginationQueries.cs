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

namespace BeanMind.Application.Documents.Queries.GetDocumentWithPagination;
public class GetDocumentWithPaginationQueries : IRequest<PaginatedList<DocumentModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetDocumentWithPaginationQueryHandler : IRequestHandler<GetDocumentWithPaginationQueries, PaginatedList<DocumentModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDocumentWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DocumentModel>> Handle(GetDocumentWithPaginationQueries request, CancellationToken cancellationToken)
    {
        var documents = _context.Get<Domain.Entities.Document>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<DocumentModel>(documents);
      
        var page = await PaginatedList<DocumentModel>.CreateAsync(map, request.PageNumber, request.PageSize);
        
        return page;
    }
}
