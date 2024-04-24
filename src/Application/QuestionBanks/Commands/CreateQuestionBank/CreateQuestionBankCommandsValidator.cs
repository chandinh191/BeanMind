using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace BeanMind.Application.QuestionBanks.Commands.CreateQuestionBank;
public class CreateQuestionBankCommandsValidator : AbstractValidator<CreateQuestionBankCommands>
{
    public CreateQuestionBankCommandsValidator()
    {
    }
}
