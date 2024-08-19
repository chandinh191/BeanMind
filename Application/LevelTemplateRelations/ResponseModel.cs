using Application.Common;
using Application.QuestionLevels;
using Application.Questions;
using Application.WorksheetTemplates;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.LevelTemplateRelations
{
    [AutoMap(typeof(Domain.Entities.LevelTemplateRelation))]
    public class GetBriefLevelTemplateRelationResponseModel : BaseResponseModel
    {
        public Guid QuestionLevelId { get; set; }
        public GetBriefQuestionLevelResponseModel QuestionLevel { get; set; }
        public Guid WorksheetTemplateId { get; set; }
        public GetBriefWorksheetTemplateResponseModel WorksheetTemplate { get; set; }
        public int NoQuestions { get; set; }
        public DateTime Created { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.LevelTemplateRelation))]
    public class GetLevelTemplateRelationResponseModel : BaseResponseModel
    {
        public Guid QuestionLevelId { get; set; }
        public GetBriefQuestionLevelResponseModel QuestionLevel { get; set; }
        public Guid WorksheetTemplateId { get; set; }
        public GetBriefWorksheetTemplateResponseModel WorksheetTemplate { get; set; }
        public int NoQuestions { get; set; }
        public DateTime Created { get; set; }
    }
}
