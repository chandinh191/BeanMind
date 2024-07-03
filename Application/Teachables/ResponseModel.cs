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
using Application.ProgramTypes;
using Application.CourseLevels;

namespace Application.Teachables
{
    [AutoMap(typeof(Domain.Entities.Teachable))]
    public class GetBriefTeachableResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid CourseLevelId { get; set; }
        public CourseLevel CourseLevel { get; set; }
        public Guid ProgramTypeId { get; set; }
        public ProgramType ProgramType { get; set; }
        public bool Status { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.Teachable))]
    public class GetTeachableResponseModel : BaseResponseModel
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid CourseLevelId { get; set; }
        public CourseLevel CourseLevel { get; set; }
        public Guid ProgramTypeId { get; set; }
        public ProgramType ProgramType { get; set; }
        public bool Status { get; set; }
    }
}
