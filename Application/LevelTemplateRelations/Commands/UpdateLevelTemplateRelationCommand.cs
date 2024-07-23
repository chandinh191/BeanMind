using Application.Common;
using Application.LevelTemplateRelations;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.LevelTemplateRelations.Commands
{
    [AutoMap(typeof(Domain.Entities.LevelTemplateRelation), ReverseMap = true)]
    public sealed record UpdateLevelTemplateRelationCommand : IRequest<BaseResponse<GetBriefLevelTemplateRelationResponseModel>>
    {
        [Required]
        public Guid Id { get; set; }
        public Guid? QuestionLevelId { get; set; }
        public Guid? WorksheetTemplateId { get; set; }
        public int? QuestionCount { get; set; }
    }

    public class UpdateLevelTemplateRelationCommandHanler : IRequestHandler<UpdateLevelTemplateRelationCommand, BaseResponse<GetBriefLevelTemplateRelationResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateLevelTemplateRelationCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefLevelTemplateRelationResponseModel>> Handle(UpdateLevelTemplateRelationCommand request, CancellationToken cancellationToken)
        {
            if (request.QuestionLevelId != null)
            {
                var questionLevel = await _context.QuestionLevels.FirstOrDefaultAsync(x => x.Id == request.QuestionLevelId);
                if (questionLevel == null)
                {
                    return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
                    {
                        Success = false,
                        Message = "Question level not found",
                    };
                }
            }

            if (request.WorksheetTemplateId != null)
            {
                var worksheetTemplate = await _context.WorksheetTemplates.FirstOrDefaultAsync(x => x.Id.Equals(request.WorksheetTemplateId));
                if (worksheetTemplate == null)
                {
                    return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
                    {
                        Success = false,
                        Message = "Worksheet template not found",
                    };
                }
            }


            var levelTemplateRelation = await _context.LevelTemplateRelations.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (levelTemplateRelation == null)
            {
                return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
                {
                    Success = false,
                    Message = "Level template relation is not found",
                    Errors = ["Level template relation is not found"]
                };
            }

            //_mapper.Map(request, enrollment);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = levelTemplateRelation.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(levelTemplateRelation, requestValue);
                    }
                }
            }

            var updateLevelTemplateRelationResult = _context.Update(levelTemplateRelation);

            if (updateLevelTemplateRelationResult.Entity == null)
            {
                return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
                {
                    Success = false,
                    Message = "Update level template relation failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedLevelTemplateRelationResult = _mapper.Map<GetBriefLevelTemplateRelationResponseModel>(updateLevelTemplateRelationResult.Entity);

            return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
            {
                Success = true,
                Message = "Update level template relation successful",
                Data = mappedLevelTemplateRelationResult
            };
        }
    }
}
