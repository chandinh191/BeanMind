using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ChapterGame : BaseAuditableEntity
    {

        [ForeignKey(nameof(Chapter))]
        public Guid ChapterId { get; set; }
        public Chapter Chapter { get; set; }

        [ForeignKey(nameof(Game))]
        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }
}
