using Application.Common;
using Application.Sessions;
using AutoMapper;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parents.Commands
{
    [AutoMap(typeof(Domain.Entities.UserEntities.Parent), ReverseMap = true)]
    public sealed record CreateParentCommand : IRequest<BaseResponse<GetBriefParentResponseModel>>
    {
        [Required]
        public string ApplicationUserId { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public double Wallet { get; set; }
        public Gender Gender { get; set; }
    }

    public class CreateParentCommandHanler : IRequestHandler<CreateParentCommand, BaseResponse<GetBriefParentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateParentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefParentResponseModel>> Handle(CreateParentCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == request.ApplicationUserId);
            if (applicationUser == null)
            {
                return new BaseResponse<GetBriefParentResponseModel>
                {
                    Success = false,
                    Message = "User not found",
                };
            }

            var parent = _mapper.Map<Domain.Entities.UserEntities.Parent>(request);
            var createParentResult = await _context.AddAsync(parent, cancellationToken);

            if (createParentResult.Entity == null)
            {
                return new BaseResponse<GetBriefParentResponseModel>
                {
                    Success = false,
                    Message = "Create parent failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedParentResult = _mapper.Map<GetBriefParentResponseModel>(createParentResult.Entity);

            return new BaseResponse<GetBriefParentResponseModel>
            {
                Success = true,
                Message = "Create parent successful",
                Data = mappedParentResult
            };
        }
    }
}
