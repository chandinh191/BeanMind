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

namespace Application.LevelTemplateRelations.Queries
{
    public sealed record GetLevelTemplateRelationQuery : IRequest<BaseResponse<GetLevelTemplateRelationResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetLevelTemplateRelationQueryHanler : IRequestHandler<GetLevelTemplateRelationQuery, BaseResponse<GetLevelTemplateRelationResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetLevelTemplateRelationQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetLevelTemplateRelationResponseModel>> Handle(GetLevelTemplateRelationQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetLevelTemplateRelationResponseModel>
                {
                    Success = false,
                    Message = "Get level template relation failed",
                    Errors = ["Id required"],
                };
            }

            var levelTemplateRelation = await _context.LevelTemplateRelations
                .Include(x => x.QuestionLevel)
                .Include(x => x.WorksheetTemplate)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedLevelTemplateRelation = _mapper.Map<GetLevelTemplateRelationResponseModel>(levelTemplateRelation);

            return new BaseResponse<GetLevelTemplateRelationResponseModel>
            {
                Success = true,
                Message = "Get level template relation successful",
                Data = mappedLevelTemplateRelation
            };
        }
    }
}
