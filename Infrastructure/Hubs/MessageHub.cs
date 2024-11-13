using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Hubs;

public class MessageHub : Hub
{
    private readonly ApplicationDbContext _context;

    public MessageHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var connection = new UserConnection
            {
                UserId = int.Parse(userId),
                ConnectionId = Context.ConnectionId
            };

            _context.UserConnections.Add(connection);
            await _context.SaveChangesAsync();
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var connection = await _context.UserConnections
                .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

            if (connection != null)
            {
                _context.UserConnections.Remove(connection);
                await _context.SaveChangesAsync();
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}