using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorksheetAttempt : BaseAuditableEntity
    {
        public string Title { get; set; }
        [ForeignKey(nameof(Enrollment))]
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }
        [ForeignKey(nameof(Worksheet))]
        public Guid WorksheetId { get; set; }
        public Worksheet Worksheet { get; set; }
        public IEnumerable<WorksheetAttemptAnswer> WorksheetAttemptAnswers { get; set; }
    }
}
