using Application.ApplicationUsers;
using Application.Common;
using Application.Participants;
using Application.TeachingSlots;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Statistic
{
    public class GetDashboardResponseModel 
    {
        public int TotalActiveCourse {  get; set; }
        public int TotalAssignedTeacher { get; set; }
        public int EnrollmentToday { get; set; }
        public int IncomeThisMonth { get; set; }
        // Initialize the list in the constructor
        public List<CourseStatisticModel> CourseStatistics { get; set; } = new List<CourseStatisticModel>();
    }
    public class CourseStatisticModel
    {
        public string Course { get; set; }
        public int TotalEnrollment { get; set; }
        public int TotalIncome { get; set; }
    }

}
