using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.UserTakeWorksheets.Commands.UpdateUserTakeWorksheet;
public class UpdateUserTakeWorksheetCommandsValidator : AbstractValidator<UpdateUserTakeWorksheetCommands>
{ 
    public UpdateUserTakeWorksheetCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
