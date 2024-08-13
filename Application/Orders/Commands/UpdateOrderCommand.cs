using Application.Common;
using Application.Parents;
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

namespace Application.Orders.Commands
{
    [AutoMap(typeof(Domain.Entities.UserEntities.Parent), ReverseMap = true)]
    public sealed record UpdateOrderCommand : IRequest<BaseResponse<GetBriefParentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public Guid? CourseId { get; set; }
        public string? ApplicationUserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public OrderStatus? Status { get; set; }
        public int? Amount { get; set; }
        public string? Provider { get; set; }
    }

    public class UpdateParentCommandHanler : IRequestHandler<UpdateOrderCommand, BaseResponse<GetBriefParentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateParentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefParentResponseModel>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (order == null)
            {
                return new BaseResponse<GetBriefParentResponseModel>
                {
                    Success = false,
                    Message = "Order is not found",
                    Errors = ["Order is not found"]
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
            if (request.CourseId != null)
            {
                var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
                if (course == null)
                {
                    return new BaseResponse<GetBriefParentResponseModel>
                    {
                        Success = false,
                        Message = "Course not found",
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
                    var targetProperty = order.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(order, requestValue);
                    }
                }
            }

            var updateOrderResult = _context.Update(order);

            if (updateOrderResult.Entity == null)
            {
                return new BaseResponse<GetBriefParentResponseModel>
                {
                    Success = false,
                    Message = "Update order failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedOrderResult = _mapper.Map<GetBriefParentResponseModel>(updateOrderResult.Entity);

            return new BaseResponse<GetBriefParentResponseModel>
            {
                Success = true,
                Message = "Update order successful",
                Data = mappedOrderResult
            };
        }
    }
}
