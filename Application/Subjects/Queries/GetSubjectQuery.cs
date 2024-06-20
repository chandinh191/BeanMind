using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Subjects.Queries;

public sealed record GetSubjectQuery : IRequest<BaseResponse<GetSubjectResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetSubjectQueryHanler : IRequestHandler<GetSubjectQuery, BaseResponse<GetSubjectResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSubjectQueryHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetSubjectResponseModel>> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            return new BaseResponse<GetSubjectResponseModel>
            {
                Success = false,
                Message = "Get subject failed",
                Errors = ["Id required"],
            };
        }

        var subject = await _context.Subject.Include(x => x.Courses).FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
        var mappedSubject = _mapper.Map<GetSubjectResponseModel>(subject);

        return new BaseResponse<GetSubjectResponseModel>
        {
            Success = true,
            Message = "Get subject successful",
            Data = mappedSubject
        };
    }
}
