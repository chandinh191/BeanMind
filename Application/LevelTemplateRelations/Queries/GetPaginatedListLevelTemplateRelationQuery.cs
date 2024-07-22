using Application.Common;
using Application.LevelTemplateRelations;
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

namespace Application.LevelTemplateRelations.Queries
{
    public sealed record GetPaginatedListLevelTemplateRelationQuery : IRequest<BaseResponse<Pagination<GetBriefLevelTemplateRelationResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid QuestionLevelId { get; set; }
        public Guid WorksheetTemplateId { get; set; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListLevelTemplateRelationQueryHandler : IRequestHandler<GetPaginatedListLevelTemplateRelationQuery, BaseResponse<Pagination<GetBriefLevelTemplateRelationResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListLevelTemplateRelationQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefLevelTemplateRelationResponseModel>>> Handle(GetPaginatedListLevelTemplateRelationQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var levelTemplateRelations = _context.LevelTemplateRelations
                .AsQueryable();
            // filter by QuestionLevelId
            if (request.QuestionLevelId != Guid.Empty)
            {
                levelTemplateRelations = levelTemplateRelations.Where(x => x.QuestionLevelId == request.QuestionLevelId);
            }

            // filter by WorksheetTemplateId
            if (request.WorksheetTemplateId != Guid.Empty)
            {
                levelTemplateRelations = levelTemplateRelations.Where(x => x.WorksheetTemplateId.Equals(request.WorksheetTemplateId));
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                levelTemplateRelations = levelTemplateRelations.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                levelTemplateRelations = levelTemplateRelations.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                levelTemplateRelations = levelTemplateRelations.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                levelTemplateRelations = levelTemplateRelations.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                levelTemplateRelations = levelTemplateRelations.Where(enroll =>
                    enroll.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                levelTemplateRelations = levelTemplateRelations.Where(enroll =>
                     enroll.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedLevelTemplateRelations = _mapper.Map<List<GetBriefLevelTemplateRelationResponseModel>>(levelTemplateRelations);
            var createPaginatedListResult = Pagination<GetBriefLevelTemplateRelationResponseModel>.Create(mappedLevelTemplateRelations.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefLevelTemplateRelationResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list level template relation failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefLevelTemplateRelationResponseModel>>
            {
                Success = true,
                Message = "Get paginated list level template relation successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
