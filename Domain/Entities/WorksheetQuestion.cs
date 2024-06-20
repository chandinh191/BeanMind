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
    public class WorksheetQuestion : BaseAuditableEntity
    {
        [Required]
        [ForeignKey(nameof(Question))]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        [Required]
        [ForeignKey(nameof(Worksheet))]
        public Guid WorksheetId { get; set; }
        public Worksheet Worksheet { get; set; }


    }
}
