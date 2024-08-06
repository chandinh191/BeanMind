using Application.Common;
using Application.Enrollments;
using Application.WorksheetAttemptAnswers;
using Application.Worksheets;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
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
        public Guid EnrollmentId { get; set; }
        public GetBriefEnrollmentResponseModel Enrollment { get; set; }
        public Guid WorksheetId { get; set; }
        public GetBriefWorksheetResponseModel Worksheet { get; set; }
        public DateTime? CompletionDate { get; set; }
        public WorksheetAttemptStatus Status { get; set; } 
        public int? Score { get; set; }
        }

    [AutoMap(typeof(Domain.Entities.WorksheetAttempt))]
    public class GetWorksheetAttemptResponseModel : BaseResponseModel
    {
        public Guid EnrollmentId { get; set; }
        public GetBriefEnrollmentResponseModel Enrollment { get; set; }
        public Guid WorksheetId { get; set; }
        public GetBriefWorksheetResponseModel Worksheet { get; set; }
        public DateTime? CompletionDate { get; set; }
        public WorksheetAttemptStatus Status { get; set; }
        public int? Score { get; set; }
        public List<GetBriefWorksheetAttemptAnswerResponseModel> WorksheetAttemptAnswers { get; set; }
    }
}
