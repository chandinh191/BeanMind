using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.UserEntities;

namespace Domain.Entities
{
    public class GameHistory : BaseAuditableEntity
    {

        [Required]
        [ForeignKey(nameof(Game))]
        public Guid GameId { get; set; }
        public Game Game { get; set; }
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public int Point {  get; set; }
        public int? Duration { get; set; }

    }
}
