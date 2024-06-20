using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Helpers;

public class LinkHelper
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly LinkGenerator _linkGenerator;

    public LinkHelper(IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
    {
        _contextAccessor = contextAccessor;
        _linkGenerator = linkGenerator;
    }

    public string generateCallbackOnRoute(string endpointName, RouteValueDictionary routeValues)
    {
        var linkResult = _linkGenerator.GetUriByName(_contextAccessor.HttpContext, endpointName, routeValues)
            ?? throw new NotSupportedException($"Could not find endpoint named '{endpointName}'.");

        return linkResult;
    }
}
