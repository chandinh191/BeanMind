using Application.Common;
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
    public sealed record UpdateParentCommand : IRequest<BaseResponse<GetBriefParentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public string? ApplicationUserId { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public double? Wallet { get; set; }
        public Gender? Gender { get; set; }
    }

    public class UpdateParentCommandHanler : IRequestHandler<UpdateParentCommand, BaseResponse<GetBriefParentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateParentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefParentResponseModel>> Handle(UpdateParentCommand request, CancellationToken cancellationToken)
        {
            var parent = await _context.Parents.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (parent == null)
            {
                return new BaseResponse<GetBriefParentResponseModel>
                {
                    Success = false,
                    Message = "Parent is not found",
                    Errors = ["Parent is not found"]
                };
            }

            if (request.ApplicationUserId != null)
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
            }
           
            //_mapper.Map(request, question);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = parent.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(parent, requestValue);
                    }
                }
            }

            var updateParentResult = _context.Update(parent);

            if (updateParentResult.Entity == null)
            {
                return new BaseResponse<GetBriefParentResponseModel>
                {
                    Success = false,
                    Message = "Update parent failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedParentResult = _mapper.Map<GetBriefParentResponseModel>(updateParentResult.Entity);

            return new BaseResponse<GetBriefParentResponseModel>
            {
                Success = true,
                Message = "Update parent successful",
                Data = mappedParentResult
            };
        }
    }
}
