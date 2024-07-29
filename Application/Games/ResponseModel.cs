using Application.ChapterGames;
using Application.Chapters;
using Application.Common;
using Application.GameHistories;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Games
{

    [AutoMap(typeof(Domain.Entities.Game))]
    public class GetBriefGameResponseModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ItemStoreJson { get; set; }
        public string AnimalJson { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Game))]
    public class GetGameResponseModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ItemStoreJson { get; set; }
        public string AnimalJson { get; set; }
        public List<GetBriefChapterGameResponseModel> ChapterGames { get; set; }
        public List<GetBriefGameHistoryResponseModel>? GameHistories { get; set; }

    }
}
