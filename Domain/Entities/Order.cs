using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Entities.UserEntities;

namespace Domain.Entities
{
    public class Order : BaseAuditableEntity
    {
        [ForeignKey(nameof(Course))]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public int Amount { get; set; }
        public string Provider { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
