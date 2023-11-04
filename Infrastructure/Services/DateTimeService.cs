using Application.Contracts;

namespace Infrastructure.Identity.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now { get; set; }
}