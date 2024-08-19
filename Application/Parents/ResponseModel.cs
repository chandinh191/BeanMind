using Application.ApplicationUsers;
using Application.Common;
using Application.Students;
using AutoMapper;
using Domain.Entities.UserEntities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parents
{
    [AutoMap(typeof(Domain.Entities.UserEntities.Parent))]
    public class GetBriefParentResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public double Wallet { get; set; }
        public Gender Gender { get; set; }
        public DateTime Created { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.UserEntities.Parent))]
    public class GetParentResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public GetBriefApplicationUserResponseModel ApplicationUser { get; set; }
        public List<GetBriefStudentResponseModel> Students { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public double Wallet { get; set; }
        public Gender Gender { get; set; }
        public DateTime Created { get; set; }
    }
}
