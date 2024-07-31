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
    public sealed record DeleteTeacherCommand : IRequest<BaseResponse<GetBriefTeacherResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteTeacherCommandHanler : IRequestHandler<DeleteTeacherCommand, BaseResponse<GetBriefTeacherResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteTeacherCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefTeacherResponseModel>> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (teacher == null)
            {
                return new BaseResponse<GetBriefTeacherResponseModel>
                {
                    Success = false,
                    Message = "Teacher not found",
                };
            }

            teacher.IsDeleted = true;

            var updateTeacherResult = _context.Update(teacher);

            if (updateTeacherResult.Entity == null)
            {
                return new BaseResponse<GetBriefTeacherResponseModel>
                {
                    Success = false,
                    Message = "Delete teacher failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTeacherResult = _mapper.Map<GetBriefTeacherResponseModel>(updateTeacherResult.Entity);
        
            return new BaseResponse<GetBriefTeacherResponseModel>
            {
                Success = true,
                Message = "Delete teacher successful",
                Data = mappedTeacherResult
            };
        }
    }
}
