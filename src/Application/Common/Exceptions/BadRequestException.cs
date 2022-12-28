using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceTemplate.Application.Common.Exceptions;
public class BadRequestException : Exception
{
    public BadRequestException()
            : base()
    {
    }

    public BadRequestException(string message)
        : base(message)
    {
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public BadRequestException(string entityType, string entityId)
        : base($"{entityType} - " +
                $"id: {entityId} not allowed.")
    {
    }
}
