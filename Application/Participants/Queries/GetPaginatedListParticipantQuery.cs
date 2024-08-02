using Application.Participants;
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
using Microsoft.EntityFrameworkCore;

namespace Application.Participants.Queries
{
    public sealed record GetPaginatedListParticipantQuery : IRequest<BaseResponse<Pagination<GetBriefParticipantResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid EnrollmentId { get; init; }
        public Guid SessionId { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListParticipantQueryHandler : IRequestHandler<GetPaginatedListParticipantQuery, BaseResponse<Pagination<GetBriefParticipantResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListParticipantQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefParticipantResponseModel>>> Handle(GetPaginatedListParticipantQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var participants = _context.Participants
                .Include(o => o.Enrollment).ThenInclude(o => o.ApplicationUser).ThenInclude(o => o.Student)
                .Include(o => o.Session).ThenInclude(o => o.TeachingSlot).ThenInclude(o => o.Course)
                .AsQueryable();

            // filter by EnrollmentId 
            if (request.EnrollmentId != Guid.Empty)
            {
                participants = participants.Where(x => x.EnrollmentId == request.EnrollmentId);
            }
            // filter by SessionId
            if (request.SessionId != Guid.Empty)
            {
                participants = participants.Where(x => x.SessionId == request.SessionId);
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                participants = participants.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                participants = participants.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                participants = participants.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                participants = participants.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                participants = participants.Where(participant =>
                    participant.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                participants = participants.Where(participant =>
                     participant.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedParticipants = _mapper.Map<List<GetBriefParticipantResponseModel>>(participants);
            var createPaginatedListResult = Pagination<GetBriefParticipantResponseModel>.Create(mappedParticipants.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefParticipantResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list participant failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefParticipantResponseModel>>
            {
                Success = true,
                Message = "Get paginated list participant successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
