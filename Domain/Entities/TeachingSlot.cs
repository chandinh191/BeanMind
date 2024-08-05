using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TeachingSlot : BaseAuditableEntity
    {
        public string? Title { get; set; }
        public int DayIndex { get; set; }
        //public int Slot { get; set; }\
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        [ForeignKey(nameof(Course))]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public IEnumerable<Session>? Sessions { get; set; }
   
    }
}
