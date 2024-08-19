using Application.Chapters;
using Application.Common;
using Application.Courses;
using Application.Games;
using Application.Topics;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChapterGames
{


    [AutoMap(typeof(Domain.Entities.ChapterGame))]
    public class GetBriefChapterGameResponseModel : BaseResponseModel
    {
        public Guid ChapterId { get; set; }
        public GetBriefChapterResponseModel Chapter { get; set; }
        public Guid GameId { get; set; }
        //public GetBriefGameResponseModel Game { get; set; }
        public DateTime Created { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.ChapterGame))]
    public class GetChapterGameResponseModel : BaseResponseModel
    {
        public Guid ChapterId { get; set; }
        public GetBriefChapterResponseModel Chapter { get; set; }
        public Guid GameId { get; set; }
        public GetBriefGameResponseModel Game { get; set; }
        public DateTime Created { get; set; }
    }

}
