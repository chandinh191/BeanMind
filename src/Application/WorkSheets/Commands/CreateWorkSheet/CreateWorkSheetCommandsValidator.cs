using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.WorkSheets.Commands.CreateWorkSheet;
public class CreateWorkSheetCommandsValidator : AbstractValidator<CreateWorkSheetCommands>
{
    public CreateWorkSheetCommandsValidator()
    {

    }
}
