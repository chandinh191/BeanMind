using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Domain.Entities;

namespace Application.WorksheetTemplates.Queries;

public sealed record GetPaginatedListWorksheetTemplateQuery : IRequest<BaseResponse<Pagination<GetBriefWorksheetTemplateResponseModel>>>
{
    public int PageIndex { get; init; }
    public int? PageSize { get; init; }
    public Guid CourseId { get; set; }
    public Guid ChapterId { get; set; }
    public Guid TopicId { get; set; }
    public string? Term { get; init; }
    public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
    public SortBy SortBy { get; init; } 
    public DateTime StartTime { get; init; } = DateTime.MinValue;
    public DateTime EndTime { get; init; } = DateTime.MinValue;
}

public class GetPaginatedListWorksheetTemplateQueryHandler : IRequestHandler<GetPaginatedListWorksheetTemplateQuery, BaseResponse<Pagination<GetBriefWorksheetTemplateResponseModel>>>
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public GetPaginatedListWorksheetTemplateQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Pagination<GetBriefWorksheetTemplateResponseModel>>> Handle(GetPaginatedListWorksheetTemplateQuery request, CancellationToken cancellationToken)
    {
        var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
        var worksheetTemplates = _context.WorksheetTemplates.AsQueryable();

        // filter by CourseId
        if (request.CourseId != Guid.Empty)
        {
            worksheetTemplates = worksheetTemplates.Where(x => x.CourseId == request.CourseId);
        }

        // filter by ChapterId
        if (request.ChapterId != Guid.Empty)
        {
            worksheetTemplates = worksheetTemplates.Where(x => x.ChapterId == request.ChapterId);
        }

        // filter by TopicId
        if (request.TopicId != Guid.Empty)
        {
            worksheetTemplates = worksheetTemplates.Where(x => x.TopicId == request.TopicId);
        }

        // filter by search Classification
        if (!string.IsNullOrEmpty(request.Term))
        {
            worksheetTemplates = worksheetTemplates.Where(x => x.Title.Contains(request.Term));
        }
       
        // filter by isDeleted
        if (request.IsDeleted.Equals(IsDeleted.Inactive))
        {
            worksheetTemplates = worksheetTemplates.Where(x => x.IsDeleted == true);
        }
        else if (request.IsDeleted.Equals(IsDeleted.Active))
        {
            worksheetTemplates = worksheetTemplates.Where(x => x.IsDeleted == false);
        }

        // filter by filterDate
        if (request.SortBy == SortBy.Ascending)
        {
            worksheetTemplates = worksheetTemplates.OrderBy(x => x.Created);
        }
        else if (request.SortBy == SortBy.Descending)
        {
            worksheetTemplates = worksheetTemplates.OrderByDescending(x => x.Created);
        }

        // filter by start time and end time
        if (request.StartTime != DateTime.MinValue)
        {
            worksheetTemplates = worksheetTemplates.Where(worksheettemplate =>
                worksheettemplate.Created >= request.StartTime );
        }
        // filter by start time and end time
        if ( request.EndTime != DateTime.MinValue)
        {
            worksheetTemplates = worksheetTemplates.Where(worksheettemplate =>
                worksheettemplate.Created <= request.EndTime);
        }

        // convert the list of item to list of response model
        var mappedWorksheetTemplates = _mapper.Map<List<GetBriefWorksheetTemplateResponseModel>>(worksheetTemplates);
        var createPaginatedListResult = Pagination<GetBriefWorksheetTemplateResponseModel>.Create(mappedWorksheetTemplates.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

        if(createPaginatedListResult == null)
        {
            return new BaseResponse<Pagination<GetBriefWorksheetTemplateResponseModel>>
            {
                Success = false,
                Message = "Get PaginatedList worksheettemplate failed",
            };
        }

        return new BaseResponse<Pagination<GetBriefWorksheetTemplateResponseModel>>
        {
            Success = true,
            Message = "Get PaginatedList worksheettemplate successful",
            Data = createPaginatedListResult,
        };
    }
}
