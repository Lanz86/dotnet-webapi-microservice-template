using MicroserviceTemplate.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace MicroserviceTemplate.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    /*public static Result<object> ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }*/
}
