using MicroserviceTemplate.Application.Common.Interfaces;
using MicroserviceTemplate.WebApi.Results;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MicroserviceTemplate.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;
    private ILogger<ApiControllerBase> _logger = null!;
    private ICurrentUserService _currentUserService = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected ILogger<ApiControllerBase> Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<ApiControllerBase>>();
    protected ICurrentUserService CurrentUserService => _currentUserService ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

    protected ApiResult<T> Result<T>(object value)
    {
        return new ApiResult<T>(value, Logger, CurrentUserService);
    }
}
