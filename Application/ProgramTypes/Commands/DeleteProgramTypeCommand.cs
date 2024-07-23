using Application.Common;
using Application.ProgramTypes;
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

namespace Application.ProgramTypes.Commands
{
    public sealed record DeleteProgramTypeCommand : IRequest<BaseResponse<GetBriefProgramTypeResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteProgramTypeCommandHanler : IRequestHandler<DeleteProgramTypeCommand, BaseResponse<GetBriefProgramTypeResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteProgramTypeCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefProgramTypeResponseModel>> Handle(DeleteProgramTypeCommand request, CancellationToken cancellationToken)
        {
            var programType = await _context.ProgramTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (programType == null)
            {
                return new BaseResponse<GetBriefProgramTypeResponseModel>
                {
                    Success = false,
                    Message = "Program type not found",
                };
            }
            programType.IsDeleted = true;
            var updateProgramTypeResult = _context.Update(programType);

            if (updateProgramTypeResult.Entity == null)
            {
                return new BaseResponse<GetBriefProgramTypeResponseModel>
                {
                    Success = false,
                    Message = "Delete program type failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedProgramTypeResult = _mapper.Map<GetBriefProgramTypeResponseModel>(updateProgramTypeResult.Entity);

            return new BaseResponse<GetBriefProgramTypeResponseModel>
            {
                Success = true,
                Message = "Delete program type successful",
                Data = mappedProgramTypeResult
            };
        }
    }
}
