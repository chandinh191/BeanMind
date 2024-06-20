using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Worksheets.Commands;

public sealed record DeleteWorksheetCommand : IRequest<BaseResponse<GetWorksheetResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteWorksheetCommandHanler : IRequestHandler<DeleteWorksheetCommand, BaseResponse<GetWorksheetResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteWorksheetCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetWorksheetResponseModel>> Handle(DeleteWorksheetCommand request, CancellationToken cancellationToken)
    {
        var worksheet = await _context.Worksheet.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(worksheet == null)
        {
            return new BaseResponse<GetWorksheetResponseModel>
            {
                Success = false,
                Message = "Worksheet not found",
            };
        }

        worksheet.IsDeleted = true;

        var updateWorksheetResult = _context.Update(worksheet);

        if (updateWorksheetResult.Entity == null)
        {
            return new BaseResponse<GetWorksheetResponseModel>
            {
                Success = false,
                Message = "Update worksheet failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedWorksheetResult = _mapper.Map<GetWorksheetResponseModel>(updateWorksheetResult.Entity);

        return new BaseResponse<GetWorksheetResponseModel>
        {
            Success = true,
            Message = "Update worksheet successful",
            Data = mappedWorksheetResult
        };
    }
}
