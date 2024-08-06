using Domain.Common;
using Domain.Enums;
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
        [ForeignKey(nameof(Enrollment))]
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }
        [ForeignKey(nameof(Worksheet))]
        public Guid WorksheetId { get; set; }
        public Worksheet Worksheet { get; set; }
        public DateTime? CompletionDate { get; set; }
        public WorksheetAttemptStatus Status { get; set; } = WorksheetAttemptStatus.NotYet; // WorksheetAttemptStatusEnum
        public int? Score { get; set; }
        public IEnumerable<WorksheetAttemptAnswer> WorksheetAttemptAnswers { get; set; }
    }
}
