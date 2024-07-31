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

namespace Application.Teachers.Queries
{
    public sealed record GetPaginatedListTeacherQuery : IRequest<BaseResponse<Pagination<GetBriefTeacherResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListTeacherQueryHandler : IRequestHandler<GetPaginatedListTeacherQuery, BaseResponse<Pagination<GetBriefTeacherResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListTeacherQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefTeacherResponseModel>>> Handle(GetPaginatedListTeacherQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var teachers = _context.Teachers.AsQueryable();


            // filter by isDeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                teachers = teachers.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                teachers = teachers.Where(x => x.IsDeleted == false);
            }

            // filter by filterDate
            if (request.SortBy == SortBy.Ascending)
            {
                teachers = teachers.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                teachers = teachers.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                teachers = teachers.Where(o =>
                    o.Created >= request.StartTime);
            }
            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                teachers = teachers.Where(o =>
                    o.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedTeachers = _mapper.Map<List<GetBriefTeacherResponseModel>>(teachers);
            var createPaginatedListResult = Pagination<GetBriefTeacherResponseModel>.Create(mappedTeachers.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefTeacherResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list teacher failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefTeacherResponseModel>>
            {
                Success = true,
                Message = "Get  paginated list teacher successful",
                Data = createPaginatedListResult,
            };
        }
    }
}
