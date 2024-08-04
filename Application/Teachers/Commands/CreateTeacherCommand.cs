using Application.Common;
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

namespace Application.Teachers.Commands
{
    [AutoMap(typeof(Domain.Entities.UserEntities.Teacher), ReverseMap = true)]
    public sealed record CreateTeacherCommand : IRequest<BaseResponse<GetBriefTeacherResponseModel>>
    {
        [Required]
        public string ApplicationUserId { get; set; }
        public string Experience { get; set; }
        public string Image { get; set; }
        public string Level { get; set; }
    }

    public class CreateTeacherCommandHanler : IRequestHandler<CreateTeacherCommand, BaseResponse<GetBriefTeacherResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTeacherCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefTeacherResponseModel>> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == request.ApplicationUserId);

            if (applicationUser == null)
            {
                return new BaseResponse<GetBriefTeacherResponseModel>
                {
                    Success = false,
                    Message = "User not found",
                };
            }

            var teacher = _mapper.Map<Domain.Entities.UserEntities.Teacher>(request);
            var createTeacherResult = await _context.AddAsync(teacher, cancellationToken);

            if (createTeacherResult.Entity == null)
            {
                return new BaseResponse<GetBriefTeacherResponseModel>
                {
                    Success = false,
                    Message = "Create teacher failed",
                };
            }

            applicationUser.TeacherId = createTeacherResult.Entity.Id;

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTeacherResult = _mapper.Map<GetBriefTeacherResponseModel>(createTeacherResult.Entity);

            return new BaseResponse<GetBriefTeacherResponseModel>
            {
                Success = true,
                Message = "Create teacher successful",
                Data = mappedTeacherResult

            };
        }
    }
}
