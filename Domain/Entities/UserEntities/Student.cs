using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserEntities
{
    public class Student : BaseAuditableEntity
    {
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; set; }
        public Parent? Parent { get; set; }
        public string? Image { get; set; }
        public string? School { get; set; }
        public string? Class { get; set; }
    }
}
