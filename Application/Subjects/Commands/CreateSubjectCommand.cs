using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.Subjects.Commands;

[AutoMap(typeof(Domain.Entities.Subject), ReverseMap = true)]
public sealed record CreateSubjectCommand : IRequest<BaseResponse<GetBriefSubjectResponseModel>>
{
    [Required]
    public string Title { get; init; }
    [Required]
    public string Description { get; init; }
}

public class CreateSubjectCommandHanler : IRequestHandler<CreateSubjectCommand, BaseResponse<GetBriefSubjectResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateSubjectCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefSubjectResponseModel>> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = _mapper.Map<Domain.Entities.Subject>(request);
        var createSubjectResult = await _context.AddAsync(subject, cancellationToken);

        if(createSubjectResult.Entity == null)
        {
            return new BaseResponse<GetBriefSubjectResponseModel>
            {
                Success = false,
                Message = "Create subject failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedSubjectResult = _mapper.Map<GetBriefSubjectResponseModel>(createSubjectResult.Entity);

        return new BaseResponse<GetBriefSubjectResponseModel>
        {
            Success = true,
            Message = "Create subject successful",
            Data = mappedSubjectResult
        };
    }
}
