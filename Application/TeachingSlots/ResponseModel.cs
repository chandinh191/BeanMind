﻿using Application.Common;
using Application.Courses;
using Application.ProgramTypes;
using Application.Sessions;
using Application.Subjects;
using Application.Teachables;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeachingSlots
{

    [AutoMap(typeof(Domain.Entities.TeachingSlot))]
    public class GetBriefTeachingSlotResponseModel : BaseResponseModel
    {
        public string Title { get; set; }
        public int DayInWeek { get; set; }
        public int Slot { get; set; }
        public Guid CourseId { get; set; }
        public GetBriefCourseResponseModel Course { get; set; }

    }

    [AutoMap(typeof(Domain.Entities.TeachingSlot))]
    public class GetTeachingSlotResponseModel : BaseResponseModel
    {
        public string Title { get; set; }
        public int DayInWeek { get; set; }
        public int Slot { get; set; }
        public Guid CourseId { get; set; }
        public GetBriefCourseResponseModel Course { get; set; }
        public List<GetBriefSessionResponseModel>? Sessions { get; set; }

    }
}
