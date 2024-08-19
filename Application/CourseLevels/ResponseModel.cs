using Application.Chapters;
using Application.Common;
using Application.Courses;
using Application.Teachables;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CourseLevels
{

    [AutoMap(typeof(Domain.Entities.CourseLevel))]
    public class GetBriefCourseLevelResponseModel : BaseResponseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<GetBriefCourseResponseModel> Courses { get; set; }
        public DateTime Created { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.CourseLevel))]
    public class GetCourseLevelResponseModel : BaseResponseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<GetBriefCourseResponseModel> Courses { get; set; }
        public List<GetBriefTeachableResponseModel> Teachables { get; set; }
        public DateTime Created { get; set; }
    }
}
