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
    public class Game : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ChapterGame> ChapterGames { get; set; }
        public IEnumerable<GameHistory>? GameHistories { get; set; }
    }
}
