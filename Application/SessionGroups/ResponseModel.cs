using Application.Common;
using Application.Enrollments;
using Application.SessionGroupRecords;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SessionGroups
{
    [AutoMap(typeof(Domain.Entities.SessionGroup))]
    public class GetBriefSessionGroupResponseModel : BaseResponseModel
    {
        public string Title { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid CourseLevelId { get; set; }
        public CourseLevel CourseLevel { get; set; }
        public Guid ProgramTypeId { get; set; }
        public ProgramType ProgramType { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.SessionGroup))]
    public class GetSessionGroupResponseModel : BaseResponseModel
    {
        public string Title { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid CourseLevelId { get; set; }
        public CourseLevel CourseLevel { get; set; }
        public Guid ProgramTypeId { get; set; }
        public ProgramType ProgramType { get; set; }
        public List<GetBriefSessionGroupRecordResponseModel> SessionGroupRecords { get; set; }
        public List<GetBriefEnrollmentResponseModel> Enrollments { get; set; }
    }
}
