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
using Application.Participants;

namespace Application.Sessions
{
    [AutoMap(typeof(Domain.Entities.Session))]
    public class GetBriefSessionResponseModel : BaseResponseModel
    {
        public DateOnly Date { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid SessionGroupRecordId { get; set; }
        public SessionGroupRecord SessionGroupRecord { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Session))]
    public class GetSessionResponseModel : BaseResponseModel
    {
        public DateOnly Date { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid SessionGroupRecordId { get; set; }
        public SessionGroupRecord SessionGroupRecord { get; set; }

        public List<GetBriefParticipantResponseModel> Participants { get; set; }
    }
}
