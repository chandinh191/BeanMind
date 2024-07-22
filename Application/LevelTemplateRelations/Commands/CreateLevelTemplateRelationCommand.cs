using Application.Common;
using Application.Enrollments;
using Application.LevelTemplateRelations;
using AutoMapper;
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
    public sealed record CreateLevelTemplateRelationCommand : IRequest<BaseResponse<GetBriefLevelTemplateRelationResponseModel>>
    {
        [Required]
        public Guid QuestionLevelId { get; set; }
        [Required]
        public Guid WorksheetTemplateId { get; set; }
        [Required]
        public int QuestionCount { get; set; }
    }

    public class CreateLevelTemplateRelationCommandHanler : IRequestHandler<CreateLevelTemplateRelationCommand, BaseResponse<GetBriefLevelTemplateRelationResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateLevelTemplateRelationCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefLevelTemplateRelationResponseModel>> Handle(CreateLevelTemplateRelationCommand request, CancellationToken cancellationToken)
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

            var worksheetTemplate = await _context.WorksheetTemplates.FirstOrDefaultAsync(x => x.Id.Equals(request.WorksheetTemplateId));
            if (worksheetTemplate == null)
            {
                return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
                {
                    Success = false,
                    Message = "Worksheet template not found",
                };
            }

            var levelTemplateRelation = _mapper.Map<Domain.Entities.LevelTemplateRelation>(request);
            var createLevelTemplateRelationResult = await _context.AddAsync(levelTemplateRelation, cancellationToken);

            if (createLevelTemplateRelationResult.Entity == null)
            {
                return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
                {
                    Success = false,
                    Message = "Create level template relation failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedLevelTemplateRelationResult = _mapper.Map<GetBriefLevelTemplateRelationResponseModel>(createLevelTemplateRelationResult.Entity);

            return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
            {
                Success = true,
                Message = "Create level template relation successful",
                Data = mappedLevelTemplateRelationResult
            };
        }
    }
}
