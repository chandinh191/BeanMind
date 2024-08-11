using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Application.Worksheets.Queries;

public sealed record GetPaginatedListWorksheetQuery : IRequest<BaseResponse<Pagination<GetBriefWorksheetResponseModel>>>
{
    public int PageIndex { get; init; }
    public int? PageSize { get; init; }
    public string? Term { get; init; }
    public Guid WorksheetTemplateId { get; init; }
    public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
    public SortBy SortBy { get; init; } 
    public DateTime StartTime { get; init; } = DateTime.MinValue;
    public DateTime EndTime { get; init; } = DateTime.MinValue;
}

public class GetPaginatedListWorksheetQueryHandler : IRequestHandler<GetPaginatedListWorksheetQuery, BaseResponse<Pagination<GetBriefWorksheetResponseModel>>>
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public GetPaginatedListWorksheetQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Pagination<GetBriefWorksheetResponseModel>>> Handle(GetPaginatedListWorksheetQuery request, CancellationToken cancellationToken)
    {
        var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
        var worksheets = _context.Worksheets
                        .Include(x => x.WorksheetTemplate)
            .Include(x => x.WorksheetQuestions)
            .AsQueryable();

        // filter by search Title and Description
        if (!string.IsNullOrEmpty(request.Term))
        {
            worksheets = worksheets.Where(x => x.Title.Contains(request.Term) || x.Description.Contains(request.Term));
        }

        // filter by WorksheetTemplateId
        if (request.WorksheetTemplateId != Guid.Empty)
        {
            worksheets = worksheets.Where(x => x.WorksheetTemplateId == request.WorksheetTemplateId);
        }

        // filter by isDeleted
        if (request.IsDeleted.Equals(IsDeleted.Inactive))
        {
            worksheets = worksheets.Where(x => x.IsDeleted == true);
        }
        else if (request.IsDeleted.Equals(IsDeleted.Inactive))
        {
            worksheets = worksheets.Where(x => x.IsDeleted == false);
        }

        // filter by filterDate
        if (request.SortBy == SortBy.Ascending)
        {
            worksheets = worksheets.OrderBy(x => x.Created);
        }
        else if (request.SortBy == SortBy.Descending)
        {
            worksheets = worksheets.OrderByDescending(x => x.Created);
        }

        // filter by start time and end time
        if (request.StartTime != DateTime.MinValue )
        {
            worksheets = worksheets.Where(worksheet =>
                worksheet.Created >= request.StartTime );
        }
        // filter by start time and end time
        if ( request.EndTime != DateTime.MinValue)
        {
            worksheets = worksheets.Where(worksheet =>
               worksheet.Created <= request.EndTime);
        }

        // convert the list of item to list of response model
        var mappedWorksheets = _mapper.Map<List<GetBriefWorksheetResponseModel>>(worksheets);
        var createPaginatedListResult = Pagination<GetBriefWorksheetResponseModel>.Create(mappedWorksheets.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

        if(createPaginatedListResult == null)
        {
            return new BaseResponse<Pagination<GetBriefWorksheetResponseModel>>
            {
                Success = false,
                Message = "Get paginated list worksheet failed",
            };
        }

        return new BaseResponse<Pagination<GetBriefWorksheetResponseModel>>
        {
            Success = true,
            Message = "Get paginated list worksheet successful",
            Data = createPaginatedListResult,
        };
    }
}
