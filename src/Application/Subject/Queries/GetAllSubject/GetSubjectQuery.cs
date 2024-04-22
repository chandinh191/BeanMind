using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BeanMind.Application.Common.Interfaces;
using BeanMind.Application.Common.Models;
using BeanMind.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using BeanMind.Application.TodoLists.Queries.GetTodos;
using BeanMind.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using MediatR;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

namespace BeanMind.Application.Subject.Queries.GetAllSubject;

public record GetSubjectQuery : IRequest<List<SubjectModel>>;

public class GetSubjectQueryHandler : IRequestHandler<GetSubjectQuery, List<SubjectModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSubjectQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SubjectModel>> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
    {


        var list = await _context.Subjects
            .AsNoTracking()
            .ProjectTo<SubjectModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        return list;
    }
}