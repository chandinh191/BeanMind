using Application.Common;
using Application.Enrollments;
using Application.WorksheetAttemptAnswers;
using Application.Worksheets;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorksheetAttempts
{
    [AutoMap(typeof(Domain.Entities.WorksheetAttempt))]
    public class GetBriefWorksheetAttemptResponseModel : BaseResponseModel
    {
        public string Title { get; set; }
        public Guid EnrollmentId { get; set; }
        //public GetBriefEnrollmentResponseModel Enrollment { get; set; }
        public Guid WorksheetId { get; set; }
        public GetBriefWorksheetResponseModel Worksheet { get; set; }

    }

    [AutoMap(typeof(Domain.Entities.WorksheetAttempt))]
    public class GetWorksheetAttemptResponseModel : BaseResponseModel
    {
        public string Title { get; set; }
        public Guid EnrollmentId { get; set; }
        public GetBriefEnrollmentResponseModel Enrollment { get; set; }
        public Guid WorksheetId { get; set; }
        public GetBriefWorksheetResponseModel Worksheet { get; set; }
        public List<GetBriefWorksheetAttemptAnswerResponseModel> WorksheetAttemptAnswers { get; set; }
    }
}
