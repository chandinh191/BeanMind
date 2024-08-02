using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Domain.Entities;
using System.Diagnostics;

namespace Application.Subjects.Queries;

public sealed record GetPaginatedListSubjectQuery : IRequest<BaseResponse<Pagination<GetBriefSubjectResponseModel>>>
{
    public int PageIndex { get; init; }
    public int? PageSize { get; init; }
    public string? Term { get; init; }
    public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
    public SortBy SortBy { get; init; } 
    public DateTime StartTime { get; init; } = DateTime.MinValue;
    public DateTime EndTime { get; init; } = DateTime.MinValue;
}

public class GetPaginatedListSubjectQueryHandler : IRequestHandler<GetPaginatedListSubjectQuery, BaseResponse<Pagination<GetBriefSubjectResponseModel>>>
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public GetPaginatedListSubjectQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Pagination<GetBriefSubjectResponseModel>>> Handle(GetPaginatedListSubjectQuery request, CancellationToken cancellationToken)
    {
        var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
        var subjects = _context.Subjects
            .Include(o => o.Courses)
            .AsQueryable();

        // filter by search Title and Description
        if (!string.IsNullOrEmpty(request.Term))
        {
            subjects = subjects.Where(x => x.Title.Contains(request.Term) || x.Description.Contains(request.Term));
        }

        // filter by isDeleted
        if (request.IsDeleted.Equals(IsDeleted.Inactive))
        {
            subjects = subjects.Where(x => x.IsDeleted == true);
        }
        else if (request.IsDeleted.Equals(IsDeleted.Active))
        {
            subjects = subjects.Where(x => x.IsDeleted == false);
        }

        // filter by filterDate
        if (request.SortBy == SortBy.Ascending)
        {
            subjects = subjects.OrderBy(x => x.Created);
        }
        else if (request.SortBy == SortBy.Descending)
        {
            subjects = subjects.OrderByDescending(x => x.Created);
        }

        // filter by start time and end time
        if (request.StartTime != DateTime.MinValue )
        {
            subjects = subjects.Where(subject =>
                subject.Created >= request.StartTime);
        }
        // filter by start time and end time
        if ( request.EndTime != DateTime.MinValue)
        {
            subjects = subjects.Where(subject =>
                subject.Created <= request.EndTime);
        }

        // convert the list of item to list of response model
        var mappedSubjects = _mapper.Map<List<GetBriefSubjectResponseModel>>(subjects);
        var createPaginatedListResult = Pagination<GetBriefSubjectResponseModel>.Create(mappedSubjects.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

        if(createPaginatedListResult == null)
        {
            return new BaseResponse<Pagination<GetBriefSubjectResponseModel>>
            {
                Success = false,
                Message = "Get paginated list subject failed",
            };
        }

        return new BaseResponse<Pagination<GetBriefSubjectResponseModel>>
        {
            Success = true,
            Message = "Get  paginated list subject successful",
            Data = createPaginatedListResult,
        };
    }
}
