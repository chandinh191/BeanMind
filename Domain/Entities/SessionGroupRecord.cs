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
    public class SessionGroupRecord : BaseAuditableEntity
    {
        [ForeignKey(nameof(SessionGroup))]
        public Guid? SessionGroupId { get; set; }
        public SessionGroup? SessionGroup { get; set; }
        public int DayInWeek { get; set; }
        public int Slot { get; set; }
    }
}
