using Application.Common;
using Application.ProgramTypes;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProgramTypes.Commands
{
    [AutoMap(typeof(Domain.Entities.ProgramType), ReverseMap = true)]
    public sealed record CreateProgramTypeCommand : IRequest<BaseResponse<GetBriefProgramTypeResponseModel>>
    {
        [Required]
        //[StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
        public string Title { get; init; }
        public string Description { get; init; }

    }

    public class CreateProgramTypeCommandHanler : IRequestHandler<CreateProgramTypeCommand, BaseResponse<GetBriefProgramTypeResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateProgramTypeCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefProgramTypeResponseModel>> Handle(CreateProgramTypeCommand request, CancellationToken cancellationToken)
        {
            var programType = _mapper.Map<Domain.Entities.ProgramType>(request);
            var createProgramTypeResult = await _context.AddAsync(programType, cancellationToken);

            if (createProgramTypeResult.Entity == null)
            {
                return new BaseResponse<GetBriefProgramTypeResponseModel>
                {
                    Success = false,
                    Message = "Create program type failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedProgramTypeResult = _mapper.Map<GetBriefProgramTypeResponseModel>(createProgramTypeResult.Entity);

            return new BaseResponse<GetBriefProgramTypeResponseModel>
            {
                Success = true,
                Message = "Create program type successful",
                Data = mappedProgramTypeResult
            };
        }
    }
}
