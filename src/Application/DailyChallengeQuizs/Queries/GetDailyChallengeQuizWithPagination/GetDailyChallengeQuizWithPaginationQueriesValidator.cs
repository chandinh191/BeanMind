using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.DailyChallengeQuizs.Queries.GetDailyChallengeQuizWithPagination;
public class GetDailyChallengeQuizWithPaginationQueriesValidator : AbstractValidator<GetDailyChallengeQuizWithPaginationQueries>
{
    public GetDailyChallengeQuizWithPaginationQueriesValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
