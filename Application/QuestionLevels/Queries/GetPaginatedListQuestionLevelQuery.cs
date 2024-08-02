using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Domain.Entities;
using System.Diagnostics;

namespace Application.QuestionLevels.Queries;

public sealed record GetPaginatedListQuestionLevelQuery : IRequest<BaseResponse<Pagination<GetQuestionLevelResponseModel>>>
{
    public int PageIndex { get; init; }
    public int? PageSize { get; init; }
    public string? Term { get; init; }
    public IsDeleted IsDeleted { get; init; } = IsDeleted.All;  
    public SortBy SortBy { get; init; }
    public DateTime StartTime { get; init; } = DateTime.MinValue;
    public DateTime EndTime { get; init; } = DateTime.MinValue;
}

public class GetPaginatedListQuestionLevelQueryHandler : IRequestHandler<GetPaginatedListQuestionLevelQuery, BaseResponse<Pagination<GetQuestionLevelResponseModel>>>
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public GetPaginatedListQuestionLevelQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Pagination<GetQuestionLevelResponseModel>>> Handle(GetPaginatedListQuestionLevelQuery request, CancellationToken cancellationToken)
    {
        var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
        var questionlevels = _context.QuestionLevels
            .Include(o => o.Questions)
            .Include(o => o.LevelTemplateRelations)
            .AsQueryable();

        // filter by search name
        if (!string.IsNullOrEmpty(request.Term))
        {
            questionlevels = questionlevels.Where(x => x.Title.Contains(request.Term) || x.Description.Contains(request.Term));
        }

        // isdeleted filter
        if (request.IsDeleted.Equals(IsDeleted.Inactive))
        {
            questionlevels = questionlevels.Where(x => x.IsDeleted == true);
        }
        else if (request.IsDeleted.Equals(IsDeleted.Active))
        {
            questionlevels = questionlevels.Where(x => x.IsDeleted == false);
        }

        // filter date Ascending or Descending
        if (request.SortBy == SortBy.Ascending)
        {
            questionlevels = questionlevels.OrderBy(x => x.Created);
        }
        else if (request.SortBy == SortBy.Descending)
        {
            questionlevels = questionlevels.OrderByDescending(x => x.Created);
        }

        // filter by start time and end time
        if (request.StartTime != DateTime.MinValue)
        {
            questionlevels = questionlevels.Where(questionlevel =>
                questionlevel.Created >= request.StartTime );
        }
        // filter by start time and end time
        if (request.EndTime != DateTime.MinValue)
        {
            questionlevels = questionlevels.Where(questionlevel =>
                questionlevel.Created <= request.EndTime);
        }

        // convert the list of item to list of response model
        var mappedQuestionLevels = _mapper.Map<List<GetQuestionLevelResponseModel>>(questionlevels);
        var createPaginatedListResult = Pagination<GetQuestionLevelResponseModel>.Create(mappedQuestionLevels.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

        if(createPaginatedListResult == null)
        {
            return new BaseResponse<Pagination<GetQuestionLevelResponseModel>>
            {
                Success = false,
                Message = "Get paginated list question level failed",
            };
        }

        return new BaseResponse<Pagination<GetQuestionLevelResponseModel>>
        {
            Success = true,
            Message = "Get paginated lis question level successful",
            Data = createPaginatedListResult,
        };
    }
}
