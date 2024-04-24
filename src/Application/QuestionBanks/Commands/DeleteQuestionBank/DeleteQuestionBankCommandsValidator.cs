using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.QuestionBanks.Commands.DeleteQuestionBank;
public class DeleteQuestionBankCommandsValidator : AbstractValidator<DeleteQuestionBankCommands>
{
    public DeleteQuestionBankCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
