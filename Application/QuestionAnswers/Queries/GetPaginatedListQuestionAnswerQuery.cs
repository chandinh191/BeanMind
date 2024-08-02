using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Application.QuestionAnswers.Queries;

public sealed record GetPaginatedListQuestionAnswerQuery : IRequest<BaseResponse<Pagination<GetBriefQuestionAnswerResponseModel>>>
{
    public int PageIndex { get; init; }
    public int? PageSize { get; init; }
    public string? Term { get; init; }
    public int? OrderIndex { get; init; }
    public Guid QuestionId { get; init; }
    public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
    public SortBy SortBy { get; init; } 
    public DateTime StartTime { get; init; } = DateTime.MinValue;
    public DateTime EndTime { get; init; } = DateTime.MinValue;
}

public class GetPaginatedListQuestionAnswerQueryHandler : IRequestHandler<GetPaginatedListQuestionAnswerQuery, BaseResponse<Pagination<GetBriefQuestionAnswerResponseModel>>>
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public GetPaginatedListQuestionAnswerQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Pagination<GetBriefQuestionAnswerResponseModel>>> Handle(GetPaginatedListQuestionAnswerQuery request, CancellationToken cancellationToken)
    {
        var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
        var questionanswers = _context.QuestionAnswers
            .Include(o => o.Question)
            .AsQueryable();

        // filter by search text
        if (!string.IsNullOrEmpty(request.Term))
        {
            questionanswers = questionanswers.Where(x => x.Content.Contains(request.Term));
        }

        // filter by question id
        if (request.QuestionId != Guid.Empty)
        {
            questionanswers = questionanswers.Where(x => x.QuestionId == request.QuestionId);
        }

        // filter by isdeleted
        if (request.IsDeleted.Equals(IsDeleted.Inactive))
        {
            questionanswers = questionanswers.Where(x => x.IsDeleted == true);
        }
        else if (request.IsDeleted.Equals(IsDeleted.Active))
        {
            questionanswers = questionanswers.Where(x => x.IsDeleted == false);
        }

        // filter by filter date
        if (request.SortBy == SortBy.Ascending)
        {
            questionanswers = questionanswers.OrderBy(x => x.Created);
        }
        else if (request.SortBy == SortBy.Descending)
        {
            questionanswers = questionanswers.OrderByDescending(x => x.Created);
        }

        // filter by start time and end time
        if (request.StartTime != DateTime.MinValue )
        {
            questionanswers = questionanswers.Where(questionanswer =>
                questionanswer.Created >= request.StartTime );
        }
        // filter by start time and end time
        if ( request.EndTime != DateTime.MinValue)
        {
            questionanswers = questionanswers.Where(questionanswer =>
                 questionanswer.Created <= request.EndTime);
        }

        // convert the list of item to list of response model
        var mappedQuestionAnswers = _mapper.Map<List<GetBriefQuestionAnswerResponseModel>>(questionanswers);
        var createPaginatedListResult = Pagination<GetBriefQuestionAnswerResponseModel>.Create(mappedQuestionAnswers.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

        if(createPaginatedListResult == null)
        {
            return new BaseResponse<Pagination<GetBriefQuestionAnswerResponseModel>>
            {
                Success = false,
                Message = "Get paginated list question answer failed",
            };
        }

        return new BaseResponse<Pagination<GetBriefQuestionAnswerResponseModel>>
        {
            Success = true,
            Message = "Get paginated list question answer successful",
            Data = createPaginatedListResult,
        };
    }
}
