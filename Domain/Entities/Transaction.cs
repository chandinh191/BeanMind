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
    public class Transaction : BaseAuditableEntity
    {
        [ForeignKey(nameof(Order))]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int Amount { get; set; }
        public string BankCode { get; set; }
        public string CardType { get; set; }
        public DateTime PayDate { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionNo { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
