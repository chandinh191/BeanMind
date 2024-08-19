using Application.Common;
using Application.Sessions;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Statistic.Queries
{
    public sealed record GetDashboardQuery : IRequest<BaseResponse<GetDashboardResponseModel>>
    {

    }

    public class GetDashboardQueryHanler : IRequestHandler<GetDashboardQuery, BaseResponse<GetDashboardResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDashboardQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetDashboardResponseModel>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var dashboardResponseModel = new GetDashboardResponseModel();

            // Materialize courses list
            var courses = _context.Courses
                .Where(o => !o.IsDeleted)
                .ToList();

            var users = _context.ApplicationUsers
                .Include(o => o.Teacher).ThenInclude(o => o.ApplicationUser).ThenInclude(o => o.Teachables)
                .Where(o => o.IsDeleted == false && o.Teacher.ApplicationUser.Teachables.Any(x => !x.IsDeleted))
                .ToList();

            DateOnly current = DateOnly.FromDateTime(DateTime.Now);

            var enrollmentsToday = _context.Enrollments
                .Where(o => DateOnly.FromDateTime(o.Created) == current)
                .Count();

            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime startOfNextMonth = startOfMonth.AddMonths(1);

            var orderAmountCount = _context.Orders
                .Where(o => !o.IsDeleted &&
                            o.Status == Domain.Enums.OrderStatus.Completed &&
                            o.Created >= startOfMonth &&
                            o.Created < startOfNextMonth)
                .Sum(o => o.Amount);

            // Set dashboard values
            dashboardResponseModel.TotalActiveCourse = courses.Count;
            dashboardResponseModel.TotalAssignedTeacher = users.Count;
            dashboardResponseModel.EnrollmentToday = enrollmentsToday;
            dashboardResponseModel.IncomeThisMonth = orderAmountCount;

            // Iterate through each course and calculate statistics
            foreach (var course in courses)
            {
                var courseStatisticModel = new CourseStatisticModel();

                // Materialize enrollments list for each course
                var enrollments = _context.Enrollments
                    .Where(o => !o.IsDeleted && o.CourseId == course.Id)
                    .ToList();

                // Calculate order amount for each course
                var courseOrderAmount = _context.Orders
                    .Where(o => !o.IsDeleted && o.Status == Domain.Enums.OrderStatus.Completed && o.CourseId == course.Id)
                    .Sum(o => o.Amount);

                courseStatisticModel.Course = course.Title;
                courseStatisticModel.TotalEnrollment = enrollments.Count;
                courseStatisticModel.TotalIncome = courseOrderAmount;

                dashboardResponseModel.CourseStatistics.Add(courseStatisticModel);
            }


            return new BaseResponse<GetDashboardResponseModel>
            {
                Success = true,
                Message = "Get session successful",
                Data = dashboardResponseModel
            };
        }
    }
}
