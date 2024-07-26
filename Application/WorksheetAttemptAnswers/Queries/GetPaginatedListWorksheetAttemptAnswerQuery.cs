using Application.Common;
using AutoMapper;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorksheetAttemptAnswers.Queries
{
    public sealed record GetPaginatedListWorksheetAttemptAnswerQuery : IRequest<BaseResponse<Pagination<GetBriefWorksheetAttemptAnswerResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid WorksheetAttemptId { get; init; }
        public Guid QuestionAnswerId { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListWorksheetAttemptAnswerQueryHandler : IRequestHandler<GetPaginatedListWorksheetAttemptAnswerQuery, BaseResponse<Pagination<GetBriefWorksheetAttemptAnswerResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListWorksheetAttemptAnswerQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefWorksheetAttemptAnswerResponseModel>>> Handle(GetPaginatedListWorksheetAttemptAnswerQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var worksheetAttemptAnswers = _context.WorksheetAttemptAnswers.AsQueryable();


            // filter by WorksheetAttemptId
            if (request.WorksheetAttemptId != Guid.Empty)
            {
                worksheetAttemptAnswers = worksheetAttemptAnswers.Where(x => x.WorksheetAttemptId == request.WorksheetAttemptId);
            }
            // filter by QuestionAnswerId
            if (request.QuestionAnswerId != Guid.Empty)
            {
                worksheetAttemptAnswers = worksheetAttemptAnswers.Where(x => x.WorksheetAttemptId == request.QuestionAnswerId);
            }

            // filter by isDeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                worksheetAttemptAnswers = worksheetAttemptAnswers.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                worksheetAttemptAnswers = worksheetAttemptAnswers.Where(x => x.IsDeleted == false);
            }

            // filter by filterDate
            if (request.SortBy == SortBy.Ascending)
            {
                worksheetAttemptAnswers = worksheetAttemptAnswers.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                worksheetAttemptAnswers = worksheetAttemptAnswers.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                worksheetAttemptAnswers = worksheetAttemptAnswers.Where(topic =>
                    topic.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                worksheetAttemptAnswers = worksheetAttemptAnswers.Where(topic =>
                   topic.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedWorksheetAttemptAnswers = _mapper.Map<List<GetBriefWorksheetAttemptAnswerResponseModel>>(worksheetAttemptAnswers);
            var createPaginatedListResult = Pagination<GetBriefWorksheetAttemptAnswerResponseModel>.Create(mappedWorksheetAttemptAnswers.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefWorksheetAttemptAnswerResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list worksheet attempt answer failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefWorksheetAttemptAnswerResponseModel>>
            {
                Success = true,
                Message = "Get paginated list worksheet attempt answer successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
