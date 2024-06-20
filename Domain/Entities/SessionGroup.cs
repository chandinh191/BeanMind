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
    public class SessionGroup : BaseAuditableEntity
    {
        public string Title { get; set; }
        
        [ForeignKey(nameof(ApplicationUser))]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
 
        [ForeignKey(nameof(CourseLevel))]
        public Guid? CourseLevelId { get; set; }
        public CourseLevel? CourseLevel { get; set; }
   
        [ForeignKey(nameof(ProgramType))]
        public Guid? ProgramTypeId { get; set; }
        public ProgramType? ProgramType { get; set; }
        public IEnumerable<SessionGroupRecord>? SessionGroupRecords { get; set; }
        public IEnumerable<Enrollment>? Enrollments { get; set; }
    }
}
