using Application.Common;
using Application.Topics;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Transactions.Commands
{

    [AutoMap(typeof(Domain.Entities.Transaction), ReverseMap = true)]
    public sealed record CreateTransactionCommand : IRequest<BaseResponse<GetBriefTransactionResponseModel>>
    {
        [Required]
        public Guid OrderId { get; set; }
        public int Amount { get; set; }
        public string BankCode { get; set; }
        public string CardType { get; set; }
        public DateTime PayDate { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionNo { get; set; }
        public TransactionStatus Status { get; set; }
    }

    public class CreateTransactionCommandHanler : IRequestHandler<CreateTransactionCommand, BaseResponse<GetBriefTransactionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTransactionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefTransactionResponseModel>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId);

            if (order == null)
            {
                return new BaseResponse<GetBriefTransactionResponseModel>
                {
                    Success = false,
                    Message = "Order not found",
                };
            }

            var transaction = _mapper.Map<Domain.Entities.Transaction>(request);
            var createTransactionResult = await _context.AddAsync(transaction, cancellationToken);

            if (createTransactionResult.Entity == null)
            {
                return new BaseResponse<GetBriefTransactionResponseModel>
                {
                    Success = false,
                    Message = "Create transaction failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTransactionResult = _mapper.Map<GetBriefTransactionResponseModel>(createTransactionResult.Entity);

            return new BaseResponse<GetBriefTransactionResponseModel>
            {
                Success = true,
                Message = "Create transaction successful",
                Data = mappedTransactionResult
            };
        }
    }

}
