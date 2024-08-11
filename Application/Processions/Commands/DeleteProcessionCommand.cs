using Application.Common;
using Application.Processions;
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

namespace Application.Processions.Commands
{
    public sealed record DeleteProcessionCommand : IRequest<BaseResponse<GetBriefProcessionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteProcessionCommandHanler : IRequestHandler<DeleteProcessionCommand, BaseResponse<GetBriefProcessionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteProcessionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefProcessionResponseModel>> Handle(DeleteProcessionCommand request, CancellationToken cancellationToken)
        {
            var procession = await _context.Processions.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (procession == null)
            {
                return new BaseResponse<GetBriefProcessionResponseModel>
                {
                    Success = false,
                    Message = "Procession not found",
                };
            }
            _context.Remove(procession);
/*            procession.IsDeleted = true;

            var updateProcessionResult = _context.Update(procession);

            if (updateProcessionResult.Entity == null)
            {
                return new BaseResponse<GetBriefProcessionResponseModel>
                {
                    Success = false,
                    Message = "Delete procession failed",
                };
            }*/

            await _context.SaveChangesAsync(cancellationToken);

            //var mappedProcessionResult = _mapper.Map<GetBriefProcessionResponseModel>(updateProcessionResult.Entity);

            return new BaseResponse<GetBriefProcessionResponseModel>
            {
                Success = true,
                Message = "Delete procession successful",
                //Data = mappedProcessionResult
            };
        }
    }
}
