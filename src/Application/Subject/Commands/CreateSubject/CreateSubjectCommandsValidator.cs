using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Subject.Commands.CreateSubject;
public class CreateSubjectCommandsValidator : AbstractValidator<CreateSubjectCommands>
{
    public CreateSubjectCommandsValidator()
    {
   
    }
}
