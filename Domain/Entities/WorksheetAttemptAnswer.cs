using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorksheetAttemptAnswer : BaseAuditableEntity
    {
        [ForeignKey(nameof(WorksheetAttempt))]
        public Guid? WorksheetAttemptId { get; set; }
        public WorksheetAttempt? WorksheetAttempt { get; set; }

        [ForeignKey(nameof(QuestionAnswer))]
        public Guid QuestionAnswerId { get; set; }
        public QuestionAnswer QuestionAnswer { get; set; }
    }
}
