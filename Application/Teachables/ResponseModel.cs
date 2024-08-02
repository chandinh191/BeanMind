using Application.Common;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Courses;
using Application.ApplicationUsers;
namespace Application.Teachables
{
    [AutoMap(typeof(Domain.Entities.Teachable))]
    public class GetBriefTeachableResponseModel : BaseResponseModel
    {
        public string? ApplicationUserId { get; set; }
        public GetBriefApplicationUserResponseModel? ApplicationUser { get; set; }
        public Guid CourseId { get; set; }
        GetBriefCourseResponseModel Course { get; set; }
        public bool Status { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Teachable))]
    public class GetTeachableResponseModel : BaseResponseModel
    {
        public string? ApplicationUserId { get; set; }
        public GetBriefApplicationUserResponseModel? ApplicationUser { get; set; }
        public Guid CourseId { get; set; }
        public GetBriefCourseResponseModel Course { get; set; }
        public bool Status { get; set; }
    }
}
