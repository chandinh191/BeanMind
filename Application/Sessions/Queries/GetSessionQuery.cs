using Application.Common;
using Application.Questions;
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

namespace Application.Sessions.Queries
{
    public sealed record GetSessionQuery : IRequest<BaseResponse<GetSessionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetSessionQueryHanler : IRequestHandler<GetSessionQuery, BaseResponse<GetSessionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSessionQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetSessionResponseModel>> Handle(GetSessionQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetSessionResponseModel>
                {
                    Success = false,
                    Message = "Get session failed",
                    Errors = ["Id required"],
                };
            }

            var session = await _context.Sessions
                .Include(x => x.ApplicationUser) .ThenInclude(o => o.Teacher)
                .Include(x => x.TeachingSlot) .ThenInclude(o => o.Course)
                .Include(x => x.Participants).ThenInclude(o =>o.Enrollment).ThenInclude(o => o.Course).ThenInclude(o => o.Chapters).ThenInclude(o => o.Topics)
                .Include(x => x.Participants).ThenInclude(o => o.Enrollment).ThenInclude(o => o.ApplicationUser).ThenInclude(o => o.Student)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedSession = _mapper.Map<GetSessionResponseModel>(session);

            return new BaseResponse<GetSessionResponseModel>
            {
                Success = true,
                Message = "Get session successful",
                Data = mappedSession
            };
        }
    }
}
