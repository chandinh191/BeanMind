using Application.Participants;
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
using Application.ApplicationUsers;
using Application.Courses;
using Domain.Enums;
using Application.WorksheetAttemptAnswers;
using Application.WorksheetAttempts;

namespace Application.Enrollments
{
    [AutoMap(typeof(Domain.Entities.Enrollment))]
    public class GetBriefEnrollmentResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public GetBriefApplicationUserResponseModel ApplicationUser { get; set; }
        public Guid CourseId { get; set; }
        public GetBriefCourseResponseModel Course { get; set; }
        public EnrollmentStatus Status { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Enrollment))]
    public class GetEnrollmentResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public GetBriefApplicationUserResponseModel ApplicationUser { get; set; }
        public Guid CourseId { get; set; }
        public GetBriefCourseResponseModel Course { get; set; }
        public List<GetBriefParticipantResponseModel> Participants { get; set; }
        public List<GetBriefWorksheetAttemptResponseModel> WorksheetAttempts { get; set; }
        public EnrollmentStatus Status { get; set; }

    }

}
