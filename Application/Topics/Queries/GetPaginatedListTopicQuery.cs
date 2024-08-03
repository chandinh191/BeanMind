using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Application.Topics.Queries;

public sealed record GetPaginatedListTopicQuery : IRequest<BaseResponse<Pagination<GetBriefTopicResponseModel>>>
{
    public int PageIndex { get; init; }
    public int? PageSize { get; init; }
    public string? Term { get; init; }
    public Guid ChapterId { get; init; }
    public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
    public SortBy SortBy { get; init; } 
    public DateTime StartTime { get; init; } = DateTime.MinValue;
    public DateTime EndTime { get; init; } = DateTime.MinValue;
}

public class GetPaginatedListTopicQueryHandler : IRequestHandler<GetPaginatedListTopicQuery, BaseResponse<Pagination<GetBriefTopicResponseModel>>>
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public GetPaginatedListTopicQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Pagination<GetBriefTopicResponseModel>>> Handle(GetPaginatedListTopicQuery request, CancellationToken cancellationToken)
    {
        var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
        var topics = _context.Topics
            .Include(o => o.Chapter)
            .AsQueryable();

        // filter by search Title and Description
        if (!string.IsNullOrEmpty(request.Term))
        {
            topics = topics.Where(x => x.Title.Contains(request.Term) || x.Description.Contains(request.Term));
        }

        // filter by ChapterId
        if (request.ChapterId != Guid.Empty)
        {
            topics = topics.Where(x => x.ChapterId == request.ChapterId);
        }

        // filter by isDeleted
        if (request.IsDeleted.Equals(IsDeleted.Inactive))
        {
            topics = topics.Where(x => x.IsDeleted == true);
        }
        else if (request.IsDeleted.Equals(IsDeleted.Active))
        {
            topics = topics.Where(x => x.IsDeleted == false);
        }

        // filter by filterDate
        if (request.SortBy == SortBy.Ascending)
        {
            topics = topics.OrderBy(x => x.Created);
        }
        else if (request.SortBy == SortBy.Descending)
        {
            topics = topics.OrderByDescending(x => x.Created);
        }

        // filter by start time and end time
        if (request.StartTime != DateTime.MinValue)
        {
            topics = topics.Where(topic =>
                topic.Created >= request.StartTime );
        }
        // filter by start time and end time
        if ( request.EndTime != DateTime.MinValue)
        {
            topics = topics.Where(topic =>
               topic.Created <= request.EndTime);
        }

        // convert the list of item to list of response model
        var mappedTopics = _mapper.Map<List<GetBriefTopicResponseModel>>(topics);
        var createPaginatedListResult = Pagination<GetBriefTopicResponseModel>.Create(mappedTopics.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

        if(createPaginatedListResult == null)
        {
            return new BaseResponse<Pagination<GetBriefTopicResponseModel>>
            {
                Success = false,
                Message = "Get paginated list topic failed",
            };
        }

        return new BaseResponse<Pagination<GetBriefTopicResponseModel>>
        {
            Success = true,
            Message = "Get paginated list topic successful",
            Data = createPaginatedListResult,
        };
    }
}
