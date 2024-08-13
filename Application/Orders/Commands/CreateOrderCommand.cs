using Application.Common;
using Application.Parents;
using AutoMapper;
using Domain.Entities.UserEntities;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Commands
{
    [AutoMap(typeof(Domain.Entities.Order), ReverseMap = true)]
    public sealed record CreateOrderCommand : IRequest<BaseResponse<GetBriefOrderResponseModel>>
    {
        [Required]
        public Guid CourseId { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public int Amount { get; set; }
        public string Provider { get; set; }
    }

    public class CreateOrderCommandHanler : IRequestHandler<CreateOrderCommand, BaseResponse<GetBriefOrderResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateOrderCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefOrderResponseModel>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
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

            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
            if (course == null)
            {
                return new BaseResponse<GetBriefOrderResponseModel>
                {
                    Success = false,
                    Message = "Course not found",
                };
            }

            var order = _mapper.Map<Domain.Entities.Order>(request);
            var createOrderResult = await _context.AddAsync(order, cancellationToken);

            if (createOrderResult.Entity == null)
            {
                return new BaseResponse<GetBriefOrderResponseModel>
                {
                    Success = false,
                    Message = "Create order failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedOrderResult = _mapper.Map<GetBriefOrderResponseModel>(createOrderResult.Entity);

            return new BaseResponse<GetBriefOrderResponseModel>
            {
                Success = true,
                Message = "Create order successful",
                Data = mappedOrderResult
            };
        }
    }
}
