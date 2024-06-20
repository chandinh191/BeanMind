using Application.Common;
using Application.ProgramTypes;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProgramTypes.Commands
{
    [AutoMap(typeof(Domain.Entities.ProgramType), ReverseMap = true)]
    public sealed record UpdateProgramTypeCommand : IRequest<BaseResponse<GetProgramTypeResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
        public string? Title { get; init; }
        [Required]
        public string? Description { get; init; }
    }

    public class UpdateProgramTypeCommandHanler : IRequestHandler<UpdateProgramTypeCommand, BaseResponse<GetProgramTypeResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProgramTypeCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetProgramTypeResponseModel>> Handle(UpdateProgramTypeCommand request, CancellationToken cancellationToken)
        {

            var programType = await _context.ProgramType.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (programType == null)
            {
                return new BaseResponse<GetProgramTypeResponseModel>
                {
                    Success = false,
                    Message = "Program type is not found",
                    Errors = ["Program type is not found"]
                };
            }

            _mapper.Map(request, programType);

            var updateProgramTypeResult = _context.Update(programType);

            if (updateProgramTypeResult.Entity == null)
            {
                return new BaseResponse<GetProgramTypeResponseModel>
                {
                    Success = false,
                    Message = "Update Program type failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedProgramTypeResult = _mapper.Map<GetProgramTypeResponseModel>(updateProgramTypeResult.Entity);

            return new BaseResponse<GetProgramTypeResponseModel>
            {
                Success = true,
                Message = "Update Program type successful",
                Data = mappedProgramTypeResult
            };
        }
    }
}
