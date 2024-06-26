﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Session : BaseAuditableEntity
    {
        public DateOnly Date { get; set; }
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        [Required]
        [ForeignKey(nameof(SessionGroup))]
        public Guid? SessionGroupId { get; set; }
        public SessionGroup? SessionGroup { get; set; }

        public IEnumerable<Participant>? Participants { get; set; }
    }
}
