using Application.Common;
using Application.Participants;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SessionGroupRecords
{
    [AutoMap(typeof(Domain.Entities.SessionGroupRecord))]
    public class GetBriefSessionGroupRecordResponseModel : BaseResponseModel
    {
        public Guid SessionGroupId { get; set; }
        public SessionGroup SessionGroup { get; set; }
        public int DayInWeek { get; set; }
        public Guid SlotId { get; set; }
        public Slot Slot { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.SessionGroupRecord))]
    public class GetSessionGroupRecordResponseModel : BaseResponseModel
    {
        public Guid SessionGroupId { get; set; }
        public SessionGroup SessionGroup { get; set; }
        public int DayInWeek { get; set; }
        public Guid SlotId { get; set; }
        public Slot Slot { get; set; }
        public List<Session> Sessions { get; set; }
    }
}
