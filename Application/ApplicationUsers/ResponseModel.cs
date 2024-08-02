using Application.Common;
using Application.Parents;
using Application.Participants;
using Application.Students;
using Application.Teachers;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationUsers
{
    [AutoMap(typeof(ApplicationUser))]
    public class GetBriefApplicationUserResponseModel : BaseResponseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }    
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Email { get; set; }
        public Guid? StudentId { get; set; }
        public GetBriefStudentResponseModel? Student { get; set; }
        public Guid? TeacherId { get; set; }
        public GetBriefTeacherResponseModel? Teacher { get; set; }
        public Guid? ParentId { get; set; }
        public GetBriefParentResponseModel? Parent { get; set; }
    }

    [AutoMap(typeof(ApplicationUser))]
    public class GetApplicationUserResponseModel : BaseResponseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Email { get; set; }
        public List<Session>? Sessions { get; set; }
        public List<Teachable>? Teachables { get; set; }
        public List<Enrollment>? Enrollments { get; set; }
        public List<GameHistory>? GameHistories { get; set; }
        public Guid? StudentId { get; set; }
        public GetBriefStudentResponseModel? Student { get; set; }
        public Guid? TeacherId { get; set; }
        public GetBriefTeacherResponseModel? Teacher { get; set; }
        public Guid? ParentId { get; set; }
        public GetBriefParentResponseModel? Parent { get; set; }

    }
}
