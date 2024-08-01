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
    public sealed record UpdateStudentCommand : IRequest<BaseResponse<GetBriefStudentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public string? ApplicationUserId { get; set; }
        public int? Image { get; set; }
        public string? School { get; set; }
        public string? Class { get; set; }
    }

    public class UpdateStudentCommandHanler : IRequestHandler<UpdateStudentCommand, BaseResponse<GetBriefStudentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateStudentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefStudentResponseModel>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (student == null)
            {
                return new BaseResponse<GetBriefStudentResponseModel>
                {
                    Success = false,
                    Message = "Student is not found",
                    Errors = ["Student is not found"]
                };
            }

            if (request.ApplicationUserId != null)
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
            }

            //_mapper.Map(request, question);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = student.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(student, requestValue);
                    }
                }
            }

            var updateStudentResult = _context.Update(student);

            if (updateStudentResult.Entity == null)
            {
                return new BaseResponse<GetBriefStudentResponseModel>
                {
                    Success = false,
                    Message = "Update student failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedStudentResult = _mapper.Map<GetBriefStudentResponseModel>(updateStudentResult.Entity);

            return new BaseResponse<GetBriefStudentResponseModel>
            {
                Success = true,
                Message = "Update student successful",
                Data = mappedStudentResult
            };
        }
    }
}
