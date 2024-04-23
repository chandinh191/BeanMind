using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Documents.Commands.CreateDocument;
public class CreateDocumentCommandsValidator : AbstractValidator<CreateDocumentCommands>
{
    public CreateDocumentCommandsValidator()
    {

    }
}
