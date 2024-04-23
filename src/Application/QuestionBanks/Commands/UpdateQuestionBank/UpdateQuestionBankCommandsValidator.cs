using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.QuestionBanks.Commands.UpdateQuestionBank;
public class UpdateQuestionBankCommandsValidator : AbstractValidator<UpdateQuestionBankCommands>
{
    public UpdateQuestionBankCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
