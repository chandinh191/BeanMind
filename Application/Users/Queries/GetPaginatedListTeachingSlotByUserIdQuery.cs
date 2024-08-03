using Application.Common;
using Application.TeachingSlots;
using AutoMapper;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public sealed record GetPaginatedListTeachingSlotByUserIdQuery : IRequest<BaseResponse<List<GetBriefTeachingSlotResponseModel>>>
    {
        [Required]
        public string Id { get; init; }
    }

    public class GetPaginatedListTeachingSlotByUserIdQueryHandler : IRequestHandler<GetPaginatedListTeachingSlotByUserIdQuery, BaseResponse<List<GetBriefTeachingSlotResponseModel>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetPaginatedListTeachingSlotByUserIdQueryHandler(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<GetBriefTeachingSlotResponseModel>>> Handle(GetPaginatedListTeachingSlotByUserIdQuery request, CancellationToken cancellationToken)
        {
            var teachingSlots = _context.TeachingSlots
                .Include(o => o.Course).ThenInclude(o => o.Enrollments)
                .Where(o => o.Course.Enrollments.Any(e => e.ApplicationUserId ==request.Id && e.Status == EnrollmentStatus.OnGoing))
                .ToList();

            // convert the list of item to list of response model
            var mappedTeachingSlots = _mapper.Map<List<GetBriefTeachingSlotResponseModel>>(teachingSlots);

            if (mappedTeachingSlots == null)
            {
                return new BaseResponse<List<GetBriefTeachingSlotResponseModel>>
                {
                    Success = false,
                    Message = "Get paginated list teaching slot failed",
                };
            }

            return new BaseResponse<List<GetBriefTeachingSlotResponseModel>>
            {
                Success = true,
                Message = "Get  paginated list teaching slot successful",
                Data = mappedTeachingSlots,
            };
        }
    }
}
