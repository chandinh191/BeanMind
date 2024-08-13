using Application.Common;
using Application.Parents;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public sealed record GetOrderQuery : IRequest<BaseResponse<GetOrderResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetParentQueryHanler : IRequestHandler<GetOrderQuery, BaseResponse<GetOrderResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetParentQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetOrderResponseModel>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetOrderResponseModel>
                {
                    Success = false,
                    Message = "Get parent failed",
                    Errors = ["Id required"],
                };
            }


            var order = await _context.Orders
                .Include(o => o.ApplicationUser).ThenInclude(o => o.Student).ThenInclude(o => o.Parent)
                .Include(o => o.Course)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            // user not found
            if (order is null)
            {
                return new BaseResponse<GetOrderResponseModel> { Success = false, Message = "Order is not existed" };
            }

            var mappedOrder = _mapper.Map<GetOrderResponseModel>(order);

            return new BaseResponse<GetOrderResponseModel>
            {
                Success = true,
                Message = "Get order successful",
                Data = mappedOrder
            };
        }
    }
}
