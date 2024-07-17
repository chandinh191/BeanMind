using Application.Common;
using Application.Games;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Games.Queries
{

    public sealed record GetPaginatedListGameQuery : IRequest<BaseResponse<Pagination<GetBriefGameResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string? Term { get; init; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListGameQueryHandler : IRequestHandler<GetPaginatedListGameQuery, BaseResponse<Pagination<GetBriefGameResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListGameQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefGameResponseModel>>> Handle(GetPaginatedListGameQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var games = _context.Games
                .AsQueryable();

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                games = games.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                games = games.Where(x => x.IsDeleted == false);
            }
            // filter by search Title and description
            if (!string.IsNullOrEmpty(request.Term))
            {
                games = games.Where(x => x.Name.Contains(request.Term) || x.Description.Contains(request.Term));
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                games = games.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                games = games.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                games = games.Where(enroll =>
                    enroll.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                games = games.Where(enroll =>
                     enroll.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedGames = _mapper.Map<List<GetBriefGameResponseModel>>(games);
            var createPaginatedListResult = Pagination<GetBriefGameResponseModel>.Create(mappedGames.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefGameResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list game failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefGameResponseModel>>
            {
                Success = true,
                Message = "Get paginated list game successful",
                Data = createPaginatedListResult,
            };
        }
    }
    }
