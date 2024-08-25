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
    public sealed record UpdateTeacherCommand : IRequest<BaseResponse<GetBriefTeacherResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public string? ApplicationUserId { get; set; }
        public string? Experience { get; set; }
        public string? Image { get; set; }
        public string? Level { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class UpdateTeacherCommandHanler : IRequestHandler<UpdateTeacherCommand, BaseResponse<GetBriefTeacherResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateTeacherCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefTeacherResponseModel>> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {

            if (request.ApplicationUserId != null)
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
            }

            var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (teacher == null)
            {
                return new BaseResponse<GetBriefTeacherResponseModel>
                {
                    Success = false,
                    Message = "Teacher is not found",
                    Errors = ["Teacher is not found"]
                };
            }

            //_mapper.Map(request, question);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = teacher.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(teacher, requestValue);
                    }
                }
            }

            var updateTeacherResult = _context.Update(teacher);

            if (updateTeacherResult.Entity == null)
            {
                return new BaseResponse<GetBriefTeacherResponseModel>
                {
                    Success = false,
                    Message = "Update teacher failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTeacherResult = _mapper.Map<GetBriefTeacherResponseModel>(updateTeacherResult.Entity);

            return new BaseResponse<GetBriefTeacherResponseModel>
            {
                Success = true,
                Message = "Update teacher successful",
                Data = mappedTeacherResult
            };
        }
    }
}
