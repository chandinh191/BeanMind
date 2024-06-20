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
/*        [ForeignKey(nameof(DayInWeek))]
        public Guid DayInWeekId { get; set; }
        public DayInWeek DayInWeek { get; set; }*/
        public int DayInWeek { get; set; }
        [ForeignKey(nameof(Slot))]
        public Guid SlotId { get; set; }
        public Slot Slot { get; set; }
        public IEnumerable<Session> Sessions { get; set; }
    }
}
