using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Procession : BaseAuditableEntity
    {
        [ForeignKey(nameof(Participant))]
        public Guid ParticipantId { get; set; }
        public Participant Participant { get; set; }
        [ForeignKey(nameof(Topic))]
        public Guid? TopicId { get; set; }
        public Topic? Topic { get; set; }
        public bool Status { get; set; }
    }
}
