﻿using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.QuestionAnswers.Queries;

public sealed record GetQuestionAnswerQuery : IRequest<BaseResponse<GetQuestionAnswerResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetQuestionAnswerQueryHanler : IRequestHandler<GetQuestionAnswerQuery, BaseResponse<GetQuestionAnswerResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuestionAnswerQueryHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionAnswerResponseModel>> Handle(GetQuestionAnswerQuery request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            return new BaseResponse<GetQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "Get question answer failed",
                Errors = ["Id required"],
            };
        }

        var questionanswer = await _context.QuestionAnswers
            .Include(x => x.Question)
            .Include(x => x.WorksheetAttemptAnswers)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

        var mappedQuestionAnswer = _mapper.Map<GetQuestionAnswerResponseModel>(questionanswer);

        return new BaseResponse<GetQuestionAnswerResponseModel>
        {
            Success = true,
            Message = "Get question answer successful",
            Data = mappedQuestionAnswer
        };
    }
}
