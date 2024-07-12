using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Subjects.Commands;

public sealed record DeleteSubjectCommand : IRequest<BaseResponse<GetSubjectResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteSubjectCommandHanler : IRequestHandler<DeleteSubjectCommand, BaseResponse<GetSubjectResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteSubjectCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetSubjectResponseModel>> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(subject == null)
        {
            return new BaseResponse<GetSubjectResponseModel>
            {
                Success = false,
                Message = "Subject not found",
            };
        }

        subject.IsDeleted = true;

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
