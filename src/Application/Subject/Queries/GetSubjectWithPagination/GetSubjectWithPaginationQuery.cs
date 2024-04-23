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

namespace BeanMind.Application.Subject.Queries.GetSubjectWithPagination;
public class GetSubjectWithPaginationQuery : IRequest<PaginatedList<SubjectModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetSubjectWithPaginationQueryHandler : IRequestHandler<GetSubjectWithPaginationQuery, PaginatedList<SubjectModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSubjectWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SubjectModel>> Handle(GetSubjectWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var comment = _context.Get<Domain.Entities.Subject>()
            .Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.ProjectTo<SubjectModel>(comment);

        var page = await PaginatedList<SubjectModel>
            .CreateAsync(map, request.PageNumber, request.PageSize);

        return page;
    }
}
