using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Domain.Entities;

namespace Application.Chapters.Queries;

public sealed record GetPaginatedListChapterQuery : IRequest<BaseResponse<Pagination<GetBriefChapterResponseModel>>>
{
    public int PageIndex { get; init; }
    public int? PageSize { get; init; }
    public string? Term { get; init; }
    public Guid CourseId { get; init; }
    public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
    public SortBy SortBy { get; init; } 
    public DateTime StartTime { get; init; } = DateTime.MinValue;
    public DateTime EndTime { get; init; } = DateTime.MinValue;
}

public class GetPaginatedListChapterQueryHandler : IRequestHandler<GetPaginatedListChapterQuery, BaseResponse<Pagination<GetBriefChapterResponseModel>>>
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public GetPaginatedListChapterQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Pagination<GetBriefChapterResponseModel>>> Handle(GetPaginatedListChapterQuery request, CancellationToken cancellationToken)
    {
        var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
        var chapters = _context.Chapters.AsQueryable();

        // filter by search Title and description
        if (!string.IsNullOrEmpty(request.Term))
        {
            chapters = chapters.Where(x => x.Title.Contains(request.Term) || x.Description.Contains(request.Term));
        }

        // filter by course id
        if (request.CourseId != Guid.Empty)
        {
            chapters = chapters.Where(x => x.CourseId == request.CourseId);
        }

        // filter by isdeleted
        if (request.IsDeleted.Equals(IsDeleted.Inactive))
        {
            chapters = chapters.Where(x => x.IsDeleted == true);
        }
        else if (request.IsDeleted.Equals(IsDeleted.Active))
        {
            chapters = chapters.Where(x => x.IsDeleted == false);
        }

        // filter by filter date
        if (request.SortBy == SortBy.Ascending)
        {
            chapters = chapters.OrderBy(x => x.Created);
        }
        else if (request.SortBy == SortBy.Descending)
        {
            chapters = chapters.OrderByDescending(x => x.Created);
        }

        // filter by start time and end time
        if (request.StartTime != DateTime.MinValue)
        {
            chapters = chapters.Where(o => o.Created >= request.StartTime);
        }

        // filter by start time and end time
        if (request.EndTime != DateTime.MinValue)
        {
            chapters = chapters.Where(o => o.Created <= request.EndTime);
        }

        // convert the list of item to list of response model
        var mappedChapters = _mapper.Map<List<GetBriefChapterResponseModel>>(chapters);
        var createPaginatedListResult = Pagination<GetBriefChapterResponseModel>.Create(mappedChapters.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

        if(createPaginatedListResult == null)
        {
            return new BaseResponse<Pagination<GetBriefChapterResponseModel>>
            {
                Success = false,
                Message = "Get PaginatedList chapter failed",
            };
        }

        return new BaseResponse<Pagination<GetBriefChapterResponseModel>>
        {
            Success = true,
            Message = "Get PaginatedList chapter successful",
            Data = createPaginatedListResult,
        };
    }
}
