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

namespace Application.Transactions.Queries
{
    public sealed record GetTransactionQuery : IRequest<BaseResponse<GetTransactionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetTransactionQueryHanler : IRequestHandler<GetTransactionQuery, BaseResponse<GetTransactionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTransactionQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetTransactionResponseModel>> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetTransactionResponseModel>
                {
                    Success = false,
                    Message = "Get transaction failed",
                    Errors = ["Id required"],
                };
            }

            var transaction = await _context.Transactions
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedTransaction = _mapper.Map<GetTransactionResponseModel>(transaction);

            return new BaseResponse<GetTransactionResponseModel>
            {
                Success = true,
                Message = "Get transaction successful",
                Data = mappedTransaction
            };
        }
    }

}
