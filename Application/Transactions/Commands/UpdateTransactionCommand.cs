using Application.Common;
using Application.Topics;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Transactions.Commands
{
    [AutoMap(typeof(Domain.Entities.Transaction), ReverseMap = true)]
    public sealed record UpdateTransactionCommand : IRequest<BaseResponse<GetBriefTransactionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public Guid? OrderId { get; set; }
        public int? Amount { get; set; }
        public string? BankCode { get; set; }
        public string? CardType { get; set; }
        public DateTime? PayDate { get; set; }
        public string? ResponseCode { get; set; }
        public string? TransactionNo { get; set; }
        public TransactionStatus? Status { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class UpdateTransactionCommandHanler : IRequestHandler<UpdateTransactionCommand, BaseResponse<GetBriefTransactionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateTransactionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefTransactionResponseModel>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            if (request.OrderId != null)
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
            }

            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (transaction == null)
            {
                return new BaseResponse<GetBriefTransactionResponseModel>
                {
                    Success = false,
                    Message = "Transaction is not found",
                    Errors = ["Transaction is not found"]
                };
            }

            //_mapper.Map(request, topic);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = transaction.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(transaction, requestValue);
                    }
                }
            }

            var updateTransactionResult = _context.Update(transaction);

            if (updateTransactionResult.Entity == null)
            {
                return new BaseResponse<GetBriefTransactionResponseModel>
                {
                    Success = false,
                    Message = "Update transaction failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTransactionResult = _mapper.Map<GetBriefTransactionResponseModel>(updateTransactionResult.Entity);

            return new BaseResponse<GetBriefTransactionResponseModel>
            {
                Success = true,
                Message = "Update transaction successful",
                Data = mappedTransactionResult
            };
        }
    }

}
