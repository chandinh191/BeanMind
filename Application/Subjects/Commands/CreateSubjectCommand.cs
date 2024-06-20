using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.Subjects.Commands;

[AutoMap(typeof(Domain.Entities.Subject), ReverseMap = true)]
public sealed record CreateSubjectCommand : IRequest<BaseResponse<GetSubjectResponseModel>>
{
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string Title { get; init; }
    [Required]
    public string Description { get; init; }
}

public class CreateSubjectCommandHanler : IRequestHandler<CreateSubjectCommand, BaseResponse<GetSubjectResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateSubjectCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetSubjectResponseModel>> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = _mapper.Map<Domain.Entities.Subject>(request);
        var createSubjectResult = await _context.AddAsync(subject, cancellationToken);

        if(createSubjectResult.Entity == null)
        {
            return new BaseResponse<GetSubjectResponseModel>
            {
                Success = false,
                Message = "Create subject failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedSubjectResult = _mapper.Map<GetSubjectResponseModel>(createSubjectResult.Entity);

        return new BaseResponse<GetSubjectResponseModel>
        {
            Success = true,
            Message = "Create subject successful",
            Data = mappedSubjectResult
        };
    }
}
