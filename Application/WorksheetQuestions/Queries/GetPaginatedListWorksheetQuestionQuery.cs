using Application.Common;

using AutoMapper;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorksheetQuestions.Queries
{
    public sealed record GetPaginatedListWorksheetQuestionQuery : IRequest<BaseResponse<Pagination<GetBriefWorksheetQuestionResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid QuestionId { get; set; }
        public Guid WorksheetId { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListWorksheetQuestionQueryHandler : IRequestHandler<GetPaginatedListWorksheetQuestionQuery, BaseResponse<Pagination<GetBriefWorksheetQuestionResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListWorksheetQuestionQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefWorksheetQuestionResponseModel>>> Handle(GetPaginatedListWorksheetQuestionQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var worksheetQuestions = _context.WorksheetQuestions
                .Include(x => x.Question)
                .Include(x => x.Worksheet)
                .AsQueryable();


            // filter by QuestionId
            if (request.QuestionId != Guid.Empty)
            {
                worksheetQuestions = worksheetQuestions.Where(x => x.QuestionId == request.QuestionId);
            }
            // filter by WorksheetId
            if (request.WorksheetId != Guid.Empty)
            {
                worksheetQuestions = worksheetQuestions.Where(x => x.WorksheetId == request.WorksheetId);
            }

            // filter by isDeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                worksheetQuestions = worksheetQuestions.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                worksheetQuestions = worksheetQuestions.Where(x => x.IsDeleted == false);
            }

            // filter by filterDate
            if (request.SortBy == SortBy.Ascending)
            {
                worksheetQuestions = worksheetQuestions.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                worksheetQuestions = worksheetQuestions.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                worksheetQuestions = worksheetQuestions.Where(topic =>
                    topic.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                worksheetQuestions = worksheetQuestions.Where(topic =>
                   topic.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedWorksheetQuestions = _mapper.Map<List<GetBriefWorksheetQuestionResponseModel>>(worksheetQuestions);
            var createPaginatedListResult = Pagination<GetBriefWorksheetQuestionResponseModel>.Create(mappedWorksheetQuestions.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefWorksheetQuestionResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list worksheet question failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefWorksheetQuestionResponseModel>>
            {
                Success = true,
                Message = "Get paginated list worksheet question successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
