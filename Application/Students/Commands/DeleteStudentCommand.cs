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
    public sealed record DeleteStudentCommand : IRequest<BaseResponse<GetBriefStudentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteStudentCommandHanler : IRequestHandler<DeleteStudentCommand, BaseResponse<GetBriefStudentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteStudentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefStudentResponseModel>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (student == null)
            {
                return new BaseResponse<GetBriefStudentResponseModel>
                {
                    Success = false,
                    Message = "Student not found",
                };
            }

            student.IsDeleted = true;

            var updateStudentResult = _context.Update(student);

            if (updateStudentResult.Entity == null)
            {
                return new BaseResponse<GetBriefStudentResponseModel>
                {
                    Success = false,
                    Message = "Delete student failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedStudentResult = _mapper.Map<GetBriefStudentResponseModel>(updateStudentResult.Entity);

            return new BaseResponse<GetBriefStudentResponseModel>
            {
                Success = true,
                Message = "Delete student successful",
                Data = mappedStudentResult
            };
        }
    }
}
