using Application.Common;
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
    public sealed record DeleteLevelTemplateRelationCommand : IRequest<BaseResponse<GetBriefLevelTemplateRelationResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteLevelTemplateRelationCommandHanler : IRequestHandler<DeleteLevelTemplateRelationCommand, BaseResponse<GetBriefLevelTemplateRelationResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteLevelTemplateRelationCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefLevelTemplateRelationResponseModel>> Handle(DeleteLevelTemplateRelationCommand request, CancellationToken cancellationToken)
        {
            var levelTemplateRelation = await _context.LevelTemplateRelations.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (levelTemplateRelation == null)
            {
                return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
                {
                    Success = false,
                    Message = "Level template relation not found",
                };
            }
            levelTemplateRelation.IsDeleted = true;

            var updateLevelTemplateRelationResult = _context.Update(levelTemplateRelation);

            if (updateLevelTemplateRelationResult.Entity == null)
            {
                return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
                {
                    Success = false,
                    Message = "Delete level template relation failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedLevelTemplateRelationResult = _mapper.Map<GetBriefLevelTemplateRelationResponseModel>(updateLevelTemplateRelationResult.Entity);

            return new BaseResponse<GetBriefLevelTemplateRelationResponseModel>
            {
                Success = true,
                Message = "Delete level template relation successful",
                Data = mappedLevelTemplateRelationResult
            };
        }
    }
}
