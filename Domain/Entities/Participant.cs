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
    public class Participant : BaseAuditableEntity
    {
        [Required]
        [ForeignKey(nameof(Enrollment))]
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }
        [Required]
        [ForeignKey(nameof(Session))]
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
        public bool IsPresent { get; set; }
    }
}
