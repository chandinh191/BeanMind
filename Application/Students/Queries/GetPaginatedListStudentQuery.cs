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

namespace Application.Students.Queries
{

    public sealed record GetPaginatedListStudentQuery : IRequest<BaseResponse<Pagination<GetBriefStudentResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string? ApplicationUserId { get; set; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListStudentQueryHandler : IRequestHandler<GetPaginatedListStudentQuery, BaseResponse<Pagination<GetBriefStudentResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListStudentQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefStudentResponseModel>>> Handle(GetPaginatedListStudentQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var students = _context.Students.AsQueryable();

            if (request.ApplicationUserId != null)
            {
                students = students.Where(x => x.ApplicationUserId == request.ApplicationUserId);
            }

            // filter by isDeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                students = students.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                students = students.Where(x => x.IsDeleted == false);
            }

            // filter by filterDate
            if (request.SortBy == SortBy.Ascending)
            {
                students = students.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                students = students.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                students = students.Where(subject =>
                    subject.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                students = students.Where(subject =>
                    subject.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedStudents = _mapper.Map<List<GetBriefStudentResponseModel>>(students);
            var createPaginatedListResult = Pagination<GetBriefStudentResponseModel>.Create(mappedStudents.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefStudentResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list student failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefStudentResponseModel>>
            {
                Success = true,
                Message = "Get paginated list student successful",
                Data = createPaginatedListResult,
            };
        }
    }

}
