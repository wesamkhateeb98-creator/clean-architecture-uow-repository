using Microsoft.AspNetCore.Mvc;

namespace Shop.Presentation.Controllers;

[ApiController]
[Route(BaseUrl)]
public abstract class BaseController : ControllerBase
{
    public const string BaseUrl = "api/v1.0/[controller]";
    public const string BaseUrlWithoutController = "api/v1.0";
}
