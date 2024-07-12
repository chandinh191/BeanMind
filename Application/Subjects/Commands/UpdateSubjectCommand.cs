using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.Subjects.Commands;

[AutoMap(typeof(Domain.Entities.Subject), ReverseMap = true)]
public sealed record UpdateSubjectCommand : IRequest<BaseResponse<GetSubjectResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string? Title { get; init; }
    [Required]
    public string? Description { get; init; }
}

public class UpdateSubjectCommandHanler : IRequestHandler<UpdateSubjectCommand, BaseResponse<GetSubjectResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateSubjectCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetSubjectResponseModel>> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(subject == null)
        {
            return new BaseResponse<GetSubjectResponseModel>
            {
                Success = false,
                Message = "Subject is not found",
                Errors = ["Subject is not found"]
            };
        }

        _mapper.Map(request, subject);

        var updateSubjectResult = _context.Update(subject);

        if (updateSubjectResult.Entity == null)
        {
            return new BaseResponse<GetSubjectResponseModel>
            {
                Success = false,
                Message = "Update subject failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedSubjectResult = _mapper.Map<GetSubjectResponseModel>(updateSubjectResult.Entity);

        return new BaseResponse<GetSubjectResponseModel>
        {
            Success = true,
            Message = "Update subject successful",
            Data = mappedSubjectResult
        };
    }
}
