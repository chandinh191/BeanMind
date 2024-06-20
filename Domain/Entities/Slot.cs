using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Slot : BaseAuditableEntity
    {
        public int SlotNumber { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public IEnumerable<SessionGroupRecord> SessionGroupRecords { get; set; }
    }
}
