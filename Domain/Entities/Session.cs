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
    public class Session : BaseAuditableEntity
    {
        public DateTime Date { get; set; }
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        [Required]
        [ForeignKey(nameof(TeachingSlot))]
        public Guid TeachingSlotId { get; set; }
        public TeachingSlot TeachingSlot { get; set; }
        public IEnumerable<Participant>? Participants { get; set; }
    }
}
