using Application.Common;
using Application.Courses;
using Application.Teachables;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProgramTypes
{
    [AutoMap(typeof(Domain.Entities.ProgramType))]
    public class GetBriefProgramTypeResponseModel : BaseResponseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        //public List<GetBriefCourseResponseModel> Courses { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.ProgramType))]
    public class GetProgramTypeResponseModel : BaseResponseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<GetBriefCourseResponseModel> Courses { get; set; }
        public List<GetBriefTeachableResponseModel> Teachables { get; set; }

    }
}
