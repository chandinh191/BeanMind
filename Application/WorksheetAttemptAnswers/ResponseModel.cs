using Application.Common;
using Application.QuestionAnswers;
using Application.Questions;
using Application.WorksheetAttempts;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorksheetAttemptAnswers
{
    [AutoMap(typeof(Domain.Entities.WorksheetAttemptAnswer))]
    public class GetBriefWorksheetAttemptAnswerResponseModel : BaseResponseModel
    {
        public Guid? WorksheetAttemptId { get; set; }
        public GetBriefWorksheetAttemptResponseModel? WorksheetAttempt { get; set; }
        public Guid QuestionAnswerId { get; set; }
        public GetBriefQuestionAnswerResponseModel QuestionAnswer { get; set; }

    }

    [AutoMap(typeof(Domain.Entities.WorksheetAttemptAnswer))]
    public class GetWorksheetAttemptAnswerResponseModel : BaseResponseModel
    {
        public Guid? WorksheetAttemptId { get; set; }
        public GetBriefWorksheetAttemptResponseModel? WorksheetAttempt { get; set; }
        public Guid QuestionAnswerId { get; set; }
        public GetBriefQuestionAnswerResponseModel QuestionAnswer { get; set; }
    }

}
