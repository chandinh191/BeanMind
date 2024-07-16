using Application.Common;
using Application.Enrollments;
using Application.Participants;
using Application.Sessions;
using Application.Topics;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Processions
{
    [AutoMap(typeof(Domain.Entities.Procession))]
    public class GetBriefProcessionResponseModel : BaseResponseModel
    {
        public Guid ParticipantId { get; set; }
        //public GetBriefParticipantResponseModel Participant { get; set; }
        public Guid? TopicId { get; set; }
        //public GetBriefTopicResponseModel? Topic { get; set; }
        public bool Status { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Procession))]
    public class GetProcessionResponseModel : BaseResponseModel
    {
        public Guid ParticipantId { get; set; }
        public GetParticipantResponseModel Participant { get; set; }
        public Guid? TopicId { get; set; }
        public GetTopicResponseModel? Topic { get; set; }
        public bool Status { get; set; }
    }

}
