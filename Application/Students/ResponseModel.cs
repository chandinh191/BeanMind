using Application.ApplicationUsers;
using Application.Common;
using AutoMapper;
using Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Students
{
    [AutoMap(typeof(Domain.Entities.UserEntities.Student))]
    public class GetBriefStudentResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public GetBriefApplicationUserResponseModel ApplicationUser { get; set; }
        public string Image { get; set; }
        public string School { get; set; }
        public string Class { get; set; }
        public Guid? ParentId { get; set; }
        public Parent? Parent { get; set; }
        public DateTime Created { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.UserEntities.Student))]
    public class GetStudentResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public GetBriefApplicationUserResponseModel ApplicationUser { get; set; }
        public string Image { get; set; }
        public string School { get; set; }
        public string Class { get; set; }
        public Guid? ParentId { get; set; }
        public Parent? Parent { get; set; }
        public DateTime Created { get; set; }
    }
}
