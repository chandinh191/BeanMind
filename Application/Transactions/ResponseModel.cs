using Application.Common;
using AutoMapper;
using Domain.Entities.UserEntities;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Transactions
{
    [AutoMap(typeof(Domain.Entities.Transaction))]
    public class GetBriefTransactionResponseModel : BaseResponseModel
    {
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

    [AutoMap(typeof(Domain.Entities.Transaction))]
    public class GetTransactionResponseModel : BaseResponseModel
    {
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
