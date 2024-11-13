using Application.Contracts;
using Coravel.Invocable;

namespace Application.BackgroundServices;

public class EventJob : IInvocable
{
    private readonly IApplicationDbContext _dbContext;

    public EventJob(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task Invoke()
    {
        var closedEvents = _dbContext.Events
            .Where(e => e.EventDate == DateTime.UtcNow.ToUniversalTime() &&
                        e.IsActive.HasValue && e.IsActive.Value);
        foreach (var closedEvent in closedEvents)
        {
            closedEvent.IsActive = false;
        }
        
        _dbContext.Events.UpdateRange(closedEvents);
         await _dbContext.SaveChangesAsync();
    }
}