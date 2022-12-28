using MediatR;
using MicroserviceTemplate.Application.Common.Exceptions;
using MicroserviceTemplate.Application.Common.Interfaces;
using MicroserviceTemplate.Application.Common.Models;
using MicroserviceTemplate.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceTemplate.WebApi.Results;

public class ApiResult<T> : ObjectResult
{

    Dictionary<Type, int> _exceptionCodes = new()
        {
                { typeof(ValidationException), StatusCodes.Status400BadRequest},
                { typeof(BadRequestException), StatusCodes.Status400BadRequest},
                { typeof(NotFoundException), StatusCodes.Status404NotFound},
                { typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized },
                { typeof(ForbiddenAccessException), StatusCodes.Status403Forbidden }
            };
    private ILogger<ApiControllerBase> _logger;
    private ICurrentUserService _currentUserService;

    public ApiResult(object? value, ILogger<ApiControllerBase> logger, ICurrentUserService currentUserService) : base(value)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async override Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(Value);
        if (Value is Result<T> resultValue)
        {
            if (resultValue.IsSuccess)
            {
                objectResult.StatusCode = (typeof(T) == typeof(Unit)) ? 204 : 200;
                objectResult.Value = resultValue.Value;
            }
            else
            {
                objectResult.StatusCode = _exceptionCodes.TryGetValue(resultValue.exception.GetType(), out int statusCode) ? statusCode : StatusCodes.Status500InternalServerError;
                objectResult.Value = new ProblemDetails
                {
                    Status = StatusCode,
                    Title = $"{resultValue}"
                };
                _logger.LogError($"Status code: {objectResult.StatusCode}. Exception: {resultValue} for user {_currentUserService.UserId}");
            }
        }
        if (objectResult.StatusCode == 204)
        {
            context.HttpContext.Response.StatusCode = 204;
            return;
        }
        else
        {
            await objectResult.ExecuteResultAsync(context);
        }
    }
}