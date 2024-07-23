using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.Subjects.Commands;

[AutoMap(typeof(Domain.Entities.Subject), ReverseMap = true)]
public sealed record UpdateSubjectCommand : IRequest<BaseResponse<GetBriefSubjectResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
}

public class UpdateSubjectCommandHanler : IRequestHandler<UpdateSubjectCommand, BaseResponse<GetBriefSubjectResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateSubjectCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefSubjectResponseModel>> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(subject == null)
        {
            return new BaseResponse<GetBriefSubjectResponseModel>
            {
                Success = false,
                Message = "Subject is not found",
                Errors = ["Subject is not found"]
            };
        }

        //_mapper.Map(request, subject);
        // Use reflection to update non-null properties
        foreach (var property in request.GetType().GetProperties())
        {
            var requestValue = property.GetValue(request);
            if (requestValue != null)
            {
                var targetProperty = subject.GetType().GetProperty(property.Name);
                if (targetProperty != null)
                {
                    targetProperty.SetValue(subject, requestValue);
                }
            }
        }

        var updateSubjectResult = _context.Update(subject);

        if (updateSubjectResult.Entity == null)
        {
            return new BaseResponse<GetBriefSubjectResponseModel>
            {
                Success = false,
                Message = "Update subject failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedSubjectResult = _mapper.Map<GetBriefSubjectResponseModel>(updateSubjectResult.Entity);

        return new BaseResponse<GetBriefSubjectResponseModel>
        {
            Success = true,
            Message = "Update subject successful",
            Data = mappedSubjectResult
        };
    }
}
