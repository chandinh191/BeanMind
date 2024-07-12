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
    public sealed record DeleteProgramTypeCommand : IRequest<BaseResponse<GetProgramTypeResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteProgramTypeCommandHanler : IRequestHandler<DeleteProgramTypeCommand, BaseResponse<GetProgramTypeResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteProgramTypeCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetProgramTypeResponseModel>> Handle(DeleteProgramTypeCommand request, CancellationToken cancellationToken)
        {
            var programType = await _context.ProgramTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (programType == null)
            {
                return new BaseResponse<GetProgramTypeResponseModel>
                {
                    Success = false,
                    Message = "Program type not found",
                };
            }
            programType.IsDeleted = true;
            var updateProgramTypeResult = _context.Update(programType);

            if (updateProgramTypeResult.Entity == null)
            {
                return new BaseResponse<GetProgramTypeResponseModel>
                {
                    Success = false,
                    Message = "Delete Program type failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedProgramTypeResult = _mapper.Map<GetProgramTypeResponseModel>(updateProgramTypeResult.Entity);

            return new BaseResponse<GetProgramTypeResponseModel>
            {
                Success = true,
                Message = "Delete Program type successful",
                Data = mappedProgramTypeResult
            };
        }
    }
}
