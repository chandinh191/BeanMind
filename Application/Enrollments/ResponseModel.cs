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

namespace Application.Enrollments
{
    [AutoMap(typeof(Domain.Entities.Enrollment))]
    public class GetBriefEnrollmentResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public Guid SessionGroupId { get; set; }
        public SessionGroup SessionGroup { get; set; }
        public bool Status { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Enrollment))]
    public class GetEnrollmentResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public Guid SessionGroupId { get; set; }
        public SessionGroup SessionGroup { get; set; }
        public List<GetBriefParticipantResponseModel> Participants { get; set; }
        public bool Status { get; set; }

    }

}
