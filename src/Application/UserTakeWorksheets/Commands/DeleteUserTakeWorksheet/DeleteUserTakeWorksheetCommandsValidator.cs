using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.UserTakeWorksheets.Commands.DeleteUserTakeWorksheet;
public class DeleteUserTakeWorksheetCommandsValidator : AbstractValidator<DeleteUserTakeWorksheetCommands>
{
    public DeleteUserTakeWorksheetCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
