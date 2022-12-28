using MicroserviceTemplate.Application.Common.Interfaces;

namespace MicroserviceTemplate.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
