using Application.Common;
using Application.SessionGroupRecords;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Slots
{
    [AutoMap(typeof(Domain.Entities.Slot))]
    public class GetBriefSlotResponseModel : BaseResponseModel
    {
        public int SlotNumber { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Slot))]
    public class GetSlotResponseModel : BaseResponseModel
    {
        public int SlotNumber { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public List<GetBriefSessionGroupRecordResponseModel> SessionGroupRecords { get; set; }
    }
}
