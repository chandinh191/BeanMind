using Application.Common;
using Application.QuestionAnswers;
using Application.QuestionLevels;
using Application.Topics;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Questions;
using Application.Worksheets;

namespace Application.WorksheetQuestions
{
    [AutoMap(typeof(Domain.Entities.WorksheetQuestion))]
    public class GetBriefWorksheetQuestionResponseModel : BaseResponseModel
    {
        public Guid QuestionId { get; set; }
        //public GetBriefQuestionResponseModel Question { get; set; }
        public Guid WorksheetId { get; set; }
        //public GetBriefWorksheetResponseModel Worksheet { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.WorksheetQuestion))]
    public class GetWorksheetQuestionResponseModel : BaseResponseModel
    {
        public Guid QuestionId { get; set; }
        public GetBriefQuestionResponseModel Question { get; set; }
        public Guid WorksheetId { get; set; }
        public GetBriefWorksheetResponseModel Worksheet { get; set; }
    }

}
