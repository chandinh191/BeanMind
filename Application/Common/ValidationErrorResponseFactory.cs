using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Application.Common;

public class ValidationErrorResponseFactory
{
    public static IActionResult Initialize(ActionContext context)
    {
        var errors = context.ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());

        var response = new BaseResponse<object>
        {
            Success = false,
            Message = "One or more validation errors occured",
            FieldErrors = errors.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
        };

        return new BadRequestObjectResult(response);
    }
}
