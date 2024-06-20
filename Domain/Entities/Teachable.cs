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
    public class Teachable : BaseAuditableEntity
    {
        [ForeignKey(nameof(ApplicationUser))]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        [Required]
        [ForeignKey(nameof(CourseLevel))]
        public Guid? CourseLevelId { get; set; }
        public CourseLevel? CourseLevel { get; set; }
        [Required]
        [ForeignKey(nameof(ProgramType))]
        public Guid? ProgramTypeId { get; set; }
        public ProgramType? ProgramType { get; set; }
        public bool Status {  get; set; }
    }
}
