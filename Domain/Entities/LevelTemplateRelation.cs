using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LevelTemplateRelation : BaseAuditableEntity
    {
        [ForeignKey(nameof(QuestionLevel))]
        public Guid QuestionLevelId { get; set; }
        public QuestionLevel QuestionLevel { get; set; }

        [ForeignKey(nameof(WorksheetTemplate))]
        public Guid WorksheetTemplateId { get; set; }
        public WorksheetTemplate WorksheetTemplate { get; set; }
        public int QuestionCount { get; set; }
    }
}
