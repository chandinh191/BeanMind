﻿using Application.Chapters;
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

namespace Application.Participants
{
    [AutoMap(typeof(Domain.Entities.Participant))]
    public class GetBriefParticipantResponseModel : BaseResponseModel
    {
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
        public bool IsPresent { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Participant))]
    public class GetParticipantResponseModel : BaseResponseModel
    {
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
        public bool IsPresent { get; set; }
    }

}
