﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class UserTakeQuiz : BaseAuditableEntity
{
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public virtual ApplicationUser? ApplicationUser { get; set; }
    [ForeignKey(nameof(Quiz))]
    public Guid QuizId { get; set; }
    public virtual Quiz? Quiz { get; set; }

    public bool IsCompleted { get; set; }
}