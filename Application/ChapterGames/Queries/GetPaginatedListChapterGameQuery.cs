using Application.ChapterGames;
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

namespace Application.ChapterGames.Queries
{

    public sealed record GetPaginatedListChapterGameQuery : IRequest<BaseResponse<Pagination<GetBriefChapterGameResponseModel>>>
    {
        public int PageIndex { get; init; }
        public int? PageSize { get; init; }
        public Guid ChapterId { get; set; }
        public Guid GameId { get; set; }
        public IsDeleted IsDeleted { get; init; } = IsDeleted.All;
        public SortBy SortBy { get; init; }
        public DateTime StartTime { get; init; } = DateTime.MinValue;
        public DateTime EndTime { get; init; } = DateTime.MinValue;
    }

    public class GetPaginatedListChapterGameQueryHandler : IRequestHandler<GetPaginatedListChapterGameQuery, BaseResponse<Pagination<GetBriefChapterGameResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListChapterGameQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Pagination<GetBriefChapterGameResponseModel>>> Handle(GetPaginatedListChapterGameQuery request, CancellationToken cancellationToken)
        {
            var defaultPageSize = _configuration.GetValue<int>("Pagination:PageSize");
            var chapterGames = _context.ChapterGames.AsQueryable();


            // filter by chapter id
            if (request.ChapterId != Guid.Empty)
            {
                chapterGames = chapterGames.Where(x => x.ChapterId == request.ChapterId);
            }
            // filter by game id
            if (request.GameId != Guid.Empty)
            {
                chapterGames = chapterGames.Where(x => x.GameId == request.GameId);
            }

            // filter by isdeleted
            if (request.IsDeleted.Equals(IsDeleted.Inactive))
            {
                chapterGames = chapterGames.Where(x => x.IsDeleted == true);
            }
            else if (request.IsDeleted.Equals(IsDeleted.Active))
            {
                chapterGames = chapterGames.Where(x => x.IsDeleted == false);
            }

            // filter by filter date
            if (request.SortBy == SortBy.Ascending)
            {
                chapterGames = chapterGames.OrderBy(x => x.Created);
            }
            else if (request.SortBy == SortBy.Descending)
            {
                chapterGames = chapterGames.OrderByDescending(x => x.Created);
            }

            // filter by start time and end time
            if (request.StartTime != DateTime.MinValue)
            {
                chapterGames = chapterGames.Where(chapter =>
                    chapter.Created >= request.StartTime);
            }

            // filter by start time and end time
            if (request.EndTime != DateTime.MinValue)
            {
                chapterGames = chapterGames.Where(chapter =>
                     chapter.Created <= request.EndTime);
            }

            // convert the list of item to list of response model
            var mappedChapterGames = _mapper.Map<List<GetBriefChapterGameResponseModel>>(chapterGames);
            var createPaginatedListResult = Pagination<GetBriefChapterGameResponseModel>.Create(mappedChapterGames.AsQueryable(), request.PageIndex, request.PageSize ?? defaultPageSize);

            if (createPaginatedListResult == null)
            {
                return new BaseResponse<Pagination<GetBriefChapterGameResponseModel>>
                {
                    Success = false,
                    Message = "Get PaginatedList chapter game failed",
                };
            }

            return new BaseResponse<Pagination<GetBriefChapterGameResponseModel>>
            {
                Success = true,
                Message = "Get PaginatedList chapter game successful",
                Data = createPaginatedListResult,
            };
        }
    }

}
