using Application.ChapterGames;
using Application.Common;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Games;
using Application.ApplicationUsers;

namespace Application.GameHistories
{
    [AutoMap(typeof(Domain.Entities.GameHistory))]
    public class GetBriefGameHistoryResponseModel : BaseResponseModel
    {
        public Guid GameId { get; set; }
        public GetBriefGameResponseModel Game { get; set; }
        public string? ApplicationUserId { get; set; }
        public GetBriefApplicationUserResponseModel? ApplicationUser { get; set; }
        public int Point { get; set; }
        public int Duration { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.GameHistory))]
    public class GetGameHistoryResponseModel : BaseResponseModel
    {
        public Guid GameId { get; set; }
        public GetBriefGameResponseModel Game { get; set; }
        public string? ApplicationUserId { get; set; }
        public GetBriefApplicationUserResponseModel? ApplicationUser { get; set; }
        public int Point { get; set; }
        public int Duration { get; set; }
    }
}
