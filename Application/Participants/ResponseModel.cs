using Application.Chapters;
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
using Application.Sessions;
using Application.Enrollments;
using Application.Processions;
using Domain.Enums;

namespace Application.Participants
{
    [AutoMap(typeof(Domain.Entities.Participant))]
    public class GetBriefParticipantResponseModel : BaseResponseModel
    {
        public Guid EnrollmentId { get; set; }
        public GetBriefEnrollmentResponseModel Enrollment { get; set; }
        public Guid SessionId { get; set; }
        public GetBriefSessionResponseModel Session { get; set; }
        public bool IsPresent { get; set; } = true;
        public ParticipantStatus Status { get; set; }

    }

    [AutoMap(typeof(Domain.Entities.Participant))]
    public class GetParticipantResponseModel : BaseResponseModel
    {
        public Guid EnrollmentId { get; set; }
        public GetBriefEnrollmentResponseModel Enrollment { get; set; }
        public Guid SessionId { get; set; }
        public GetBriefSessionResponseModel Session { get; set; }
        public List<GetBriefProcessionResponseModel> Processions { get; set; }
        public bool IsPresent { get; set; }
        public ParticipantStatus Status { get; set; }
    }

}
