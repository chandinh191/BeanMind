using Application.ApplicationUsers;
using Application.Common;
using Application.Courses;
using AutoMapper;
using Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Teachers
{
    [AutoMap(typeof(Domain.Entities.UserEntities.Teacher))]
    public class GetBriefTeacherResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public string? Experience { get; set; }
        public string? Image { get; set; }
        public string? Level { get; set; }
        public DateTime Created { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.UserEntities.Teacher))]
    public class GetTeacherResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public GetBriefApplicationUserResponseModel ApplicationUser { get; set; }
        public string? Experience { get; set; }
        public string? Image { get; set; }
        public string? Level { get; set; }
        public DateTime Created { get; set; }
    }
}
