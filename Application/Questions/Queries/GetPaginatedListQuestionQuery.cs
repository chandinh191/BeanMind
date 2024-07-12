using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;
using System.Diagnostics;

namespace Application.Questions.Queries;

public sealed record GetPaginatedListQuestionQuery : IRequest<BaseResponse<Pagination<GetBriefQuestionResponseModel>>>
{
    public int PageIndex { get; init; }
    public int? PageSize { get; init; }
    public string? Term { get; init; }
    public Guid TopicId { get; init; }
    public Guid QuestionLevelId { get; init; }
    public Guid QuestionTypeId { get; init; }
    public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
    public SortBy SortBy { get; init; } 
    public DateTime StartTime { get; init; } = DateTime.MinValue;
    public DateTime EndTime { get; init; } = DateTime.MinValue;
}

public class GetPaginatedListQuestionQueryHandler : IRequestHandler<GetPaginatedListQuestionQuery, BaseResponse<Pagination<GetBriefQuestionResponseModel>>>
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public GetPaginatedListQuestionQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Pagination<GetBriefQuestionResponseModel>>> Handle(GetPaginatedListQuestionQuery request, CancellationToken cancellationToken)
    {
        var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
        var questions = _context.Questions.AsQueryable();

        if (!string.IsNullOrEmpty(request.Term))
        {
            questions = questions.Where(x => x.Content.Contains(request.Term));
        }

        if (request.TopicId != Guid.Empty)
        {
            questions = questions.Where(x => x.TopicId == request.TopicId);
        }

        if (request.QuestionLevelId != Guid.Empty)
        {
            questions = questions.Where(x => x.QuestionLevelId == request.QuestionLevelId);
        }

       

        // isdeleted filter
        if (request.IsDeleted.Equals(IsDeleted.Inactive))
        {
            questions = questions.Where(x => x.IsDeleted == true);
        }
        else if (request.IsDeleted.Equals(IsDeleted.Active))
        {
            questions = questions.Where(x => x.IsDeleted == false);
        }

        // filter date Ascending or Descending
        if (request.SortBy == SortBy.Ascending)
        {
            questions = questions.OrderBy(x => x.Created);
        }
        else if (request.SortBy == SortBy.Descending)
        {
            questions = questions.OrderByDescending(x => x.Created);
        }

        // filter by start time and end time
        if (request.StartTime != DateTime.MinValue)
        {
            questions = questions.Where(question =>
                question.Created >= request.StartTime );
        }
        // filter by start time and end time
        if ( request.EndTime != DateTime.MinValue)
        {
            questions = questions.Where(question =>
                 question.Created <= request.EndTime);
        }

        // convert the list of item to list of response model
        var mappedQuestions = _mapper.Map<List<GetBriefQuestionResponseModel>>(questions);
        var createPaginatedListResult = Pagination<GetBriefQuestionResponseModel>.Create(mappedQuestions.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

        if(createPaginatedListResult == null)
        {
            return new BaseResponse<Pagination<GetBriefQuestionResponseModel>>
            {
                Success = false,
                Message = "Get PaginatedList question failed",
            };
        }

        return new BaseResponse<Pagination<GetBriefQuestionResponseModel>>
        {
            Success = true,
            Message = "Get PaginatedList question successful",
            Data = createPaginatedListResult,
        };
    }
}
