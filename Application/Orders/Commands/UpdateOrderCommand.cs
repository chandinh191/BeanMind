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
    public sealed record UpdateOrderCommand : IRequest<BaseResponse<GetBriefOrderResponseModel>>
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

    public class UpdateParentCommandHanler : IRequestHandler<UpdateOrderCommand, BaseResponse<GetBriefOrderResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateParentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefOrderResponseModel>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (order == null)
            {
                return new BaseResponse<GetBriefOrderResponseModel>
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
                    return new BaseResponse<GetBriefOrderResponseModel>
                    {
                        Success = false,
                        Message = "User not found",
                    };
                }
            }
            int coursePrice = 0;
            if (request.CourseId != null)
            {
                var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
                if (course == null)
                {
                    return new BaseResponse<GetBriefOrderResponseModel>
                    {
                        Success = false,
                        Message = "Course not found",
                    };
                }
                coursePrice = course.Price;
            }
            if (request.Status != null && request.Status == OrderStatus.Completed)
            {
                var money = _context.Transactions
                    .Where(o => o.OrderId == order.Id && o.Status == TransactionStatus.Success)
                    .Sum(t => t.Amount);

                if (money != null && money < coursePrice)
                {
                    return new BaseResponse<GetBriefOrderResponseModel>
                    {
                        Success = false,
                        Message = "You need to finish enough transaction to update order to success",
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
                return new BaseResponse<GetBriefOrderResponseModel>
                {
                    Success = false,
                    Message = "Update order failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedOrderResult = _mapper.Map<GetBriefOrderResponseModel>(updateOrderResult.Entity);

            return new BaseResponse<GetBriefOrderResponseModel>
            {
                Success = true,
                Message = "Update order successful",
                Data = mappedOrderResult
            };
        }
    }
}
