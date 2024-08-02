using Application.Common;
using Application.ProgramTypes;
using AutoMapper;
using Domain.Entities;
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
    public sealed record UpdateProgramTypeCommand : IRequest<BaseResponse<GetBriefProgramTypeResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        //[StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
        public string? Title { get; init; }
        public string? Description { get; init; }
    }

    public class UpdateProgramTypeCommandHanler : IRequestHandler<UpdateProgramTypeCommand, BaseResponse<GetBriefProgramTypeResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProgramTypeCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefProgramTypeResponseModel>> Handle(UpdateProgramTypeCommand request, CancellationToken cancellationToken)
        {

            var programType = await _context.ProgramTypes.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (programType == null)
            {
                return new BaseResponse<GetBriefProgramTypeResponseModel>
                {
                    Success = false,
                    Message = "Program type is not found",
                    Errors = ["Program type is not found"]
                };
            }

            //_mapper.Map(request, programType);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = programType.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(programType, requestValue);
                    }
                }
            }

            var updateProgramTypeResult = _context.Update(programType);

            if (updateProgramTypeResult.Entity == null)
            {
                return new BaseResponse<GetBriefProgramTypeResponseModel>
                {
                    Success = false,
                    Message = "Update program type failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedProgramTypeResult = _mapper.Map<GetBriefProgramTypeResponseModel>(updateProgramTypeResult.Entity);

            return new BaseResponse<GetBriefProgramTypeResponseModel>
            {
                Success = true,
                Message = "Update program type successful",
                Data = mappedProgramTypeResult
            };
        }
    }
}
