using Application.Common;
using Application.QuestionLevels;
using Application.WorksheetTemplates;
using AutoMapper;
using Domain.Entities.UserEntities;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Transactions;

namespace Application.Orders
{
    [AutoMap(typeof(Domain.Entities.Order))]
    public class GetBriefOrderResponseModel : BaseResponseModel
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public int Amount { get; set; }
        public string Provider { get; set; }
        public DateTime Created { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Order))]
    public class GetOrderResponseModel : BaseResponseModel
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public int Amount { get; set; }
        public string Provider { get; set; }
        public List<GetBriefTransactionResponseModel> Transactions { get; set; }
        public DateTime Created { get; set; }
    }
}
