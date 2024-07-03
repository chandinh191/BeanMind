﻿using Domain.Common;
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
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        [Required]
        [ForeignKey(nameof(Course))]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public bool Status {  get; set; }
    }
}
