using Application.Common;
using Application.Topics;
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

namespace Application.Transactions.Commands
{

    public sealed record DeleteTransactionCommand : IRequest<BaseResponse<GetBriefTransactionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteTransactionCommandHanler : IRequestHandler<DeleteTransactionCommand, BaseResponse<GetBriefTransactionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteTransactionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefTransactionResponseModel>> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (transaction == null)
            {
                return new BaseResponse<GetBriefTransactionResponseModel>
                {
                    Success = false,
                    Message = "Transaction not found",
                };
            }

            transaction.IsDeleted = true;

            var updateTransactionResult = _context.Update(transaction);

            if (updateTransactionResult.Entity == null)
            {
                return new BaseResponse<GetBriefTransactionResponseModel>
                {
                    Success = false,
                    Message = "Delete transaction failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTransactionResult = _mapper.Map<GetBriefTransactionResponseModel>(updateTransactionResult.Entity);

            return new BaseResponse<GetBriefTransactionResponseModel>
            {
                Success = true,
                Message = "Delete transaction successful",
                Data = mappedTransactionResult
            };
        }
    }

}
