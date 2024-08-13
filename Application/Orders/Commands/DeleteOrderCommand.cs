using Application.Common;
using Application.Parents;
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

namespace Application.Orders.Commands
{
    public sealed record DeleteOrderCommand : IRequest<BaseResponse<GetBriefOrderResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteOrderCommandHanler : IRequestHandler<DeleteOrderCommand, BaseResponse<GetBriefOrderResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteOrderCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefOrderResponseModel>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (order == null)
            {
                return new BaseResponse<GetBriefOrderResponseModel>
                {
                    Success = false,
                    Message = "Order not found",
                };
            }

            order.IsDeleted = true;

            var updateOrderResult = _context.Update(order);

            if (updateOrderResult.Entity == null)
            {
                return new BaseResponse<GetBriefOrderResponseModel>
                {
                    Success = false,
                    Message = "Delete order failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedOrderResult = _mapper.Map<GetBriefOrderResponseModel>(updateOrderResult.Entity);

            return new BaseResponse<GetBriefOrderResponseModel>
            {
                Success = true,
                Message = "Delete order successful",
                Data = mappedOrderResult
            };
        }
    }
}
