using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.UserTakeWorksheets.Commands.CreateUserTakeWorksheet;
public class CreateUserTakeWorksheetCommandsValidator : AbstractValidator<CreateUserTakeWorksheetCommands>
{
    public CreateUserTakeWorksheetCommandsValidator()
    {

    }
}
