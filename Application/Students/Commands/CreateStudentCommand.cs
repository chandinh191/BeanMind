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

namespace Application.Students.Commands
{
    [AutoMap(typeof(Domain.Entities.UserEntities.Student), ReverseMap = true)]
    public sealed record CreateStudentCommand : IRequest<BaseResponse<GetBriefStudentResponseModel>>
    {
        [Required]
        public string ApplicationUserId { get; set; }
        public Guid? ParentId { get; set; }
        public int Image { get; set; }
        public string School { get; set; }
        public string Class { get; set; }
    }

    public class CreateStudentCommandHanler : IRequestHandler<CreateStudentCommand, BaseResponse<GetBriefStudentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateStudentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefStudentResponseModel>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == request.ApplicationUserId);
            if (applicationUser == null)
            {
                return new BaseResponse<GetBriefStudentResponseModel>
                {
                    Success = false,
                    Message = "User not found",
                };
            }
            if (request.ParentId != null)
            {
                var parent = await _context.Parents.FirstOrDefaultAsync(x => x.Id == request.ParentId);
                if (parent == null)
                {
                    return new BaseResponse<GetBriefStudentResponseModel>
                    {
                        Success = false,
                        Message = "Parent not found",
                    };
                }
            }

            var student = _mapper.Map<Domain.Entities.UserEntities.Student>(request);
            var createStudentResult = await _context.AddAsync(student, cancellationToken);

            if (createStudentResult.Entity == null)
            {
                return new BaseResponse<GetBriefStudentResponseModel>
                {
                    Success = false,
                    Message = "Create student failed",
                };
            }

            applicationUser.StudentId = createStudentResult.Entity.Id;
            await _context.SaveChangesAsync(cancellationToken);

            var mappedStudentResult = _mapper.Map<GetBriefStudentResponseModel>(createStudentResult.Entity);

            return new BaseResponse<GetBriefStudentResponseModel>
            {
                Success = true,
                Message = "Create student successful",
                Data = mappedStudentResult
            };
        }
    }
}
