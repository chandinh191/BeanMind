using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.DailyChallengeQuestions.Commands.DeleteDailyChallengeQuestions;
public class DeleteDailyChallengeQuestionCommandsValidator : AbstractValidator<DeleteDailyChallengeQuestionCommands>
{
    public DeleteDailyChallengeQuestionCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
