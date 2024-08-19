using Application.Common;
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

namespace Application.Teachables.Commands
{
    public sealed record DeleteTeachableCommand : IRequest<BaseResponse<GetBriefTeachableResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteTeachableCommandHanler : IRequestHandler<DeleteTeachableCommand, BaseResponse<GetBriefTeachableResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteTeachableCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefTeachableResponseModel>> Handle(DeleteTeachableCommand request, CancellationToken cancellationToken)
        {
            var teachable = await _context.Teachables.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (teachable == null)
            {
                return new BaseResponse<GetBriefTeachableResponseModel>
                {
                    Success = false,
                    Message = "Teachable not found",
                };
            }


            //teachable.IsDeleted = true;

            var updateTeachableResult = _context.Remove(teachable);

            if (updateTeachableResult.Entity == null)
            {
                return new BaseResponse<GetBriefTeachableResponseModel>
                {
                    Success = false,
                    Message = "Delete teachable failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTeachableResult = _mapper.Map<GetBriefTeachableResponseModel>(updateTeachableResult.Entity);

            return new BaseResponse<GetBriefTeachableResponseModel>
            {
                Success = true,
                Message = "Delete teachable successful",
                Data = mappedTeachableResult
            };
        }
    }
}
