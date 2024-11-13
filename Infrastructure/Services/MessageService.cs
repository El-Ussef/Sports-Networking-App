using Application.Contracts;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Dtos;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Services;

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<MessageHub> _hubContext;

    public MessageService(ApplicationDbContext context, IHubContext<MessageHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<int> CreateMessageAsync(int senderId, int receiverId, string content)
    {
        var message = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        var connections = await _context.UserConnections
            .Where(c => c.UserId == receiverId)
            .ToListAsync();

        foreach (var connection in connections)
        {
            await _hubContext.Clients.Client(connection.ConnectionId).SendAsync("ReceiveMessage", message);
        }

        return message.Id;
    }

    public async Task<List<MessageVM>> GetMessagesAsync(int senderId, int receiverId, int pageNumber, int pageSize)
    {
        return await _context.Messages
            .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                        (m.SenderId == receiverId && m.ReceiverId == senderId))
            .OrderBy(m => m.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new MessageVM
            {
                Id = c.Id,
                Content = c.Content,
                IsRead = c.IsRead,
                Timestamp = c.Timestamp,
                Sender = c.Sender.FirstName + "" + c.Sender.LastName,
                SenderId = c.SenderId
                
            })
            .ToListAsync();
    }

    public async Task<List<MessageOverViewVM>> GetAllMessagesAsync(int userId, int pageNumber, int pageSize)
    {
        var messages = await _context.Messages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .Include(c => c.Sender)
            .ToListAsync();

        var distinctMessages = messages
            .GroupBy(m => new { 
                ConversationId = m.SenderId < m.ReceiverId ? 
                    m.SenderId + "-" + m.ReceiverId : 
                    m.ReceiverId + "-" + m.SenderId 
            })
            .Select(g => g.OrderByDescending(m => m.Timestamp).First())
            .OrderByDescending(m => m.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new MessageOverViewVM
            {
                SenderPicturePath = c.SenderId == userId ? c.Receiver?.ProfilePicturePath : c.Sender?.ProfilePicturePath,
                SenderId = c.SenderId == userId ? c.ReceiverId : c.SenderId,
                ReceiverId = userId, // Always set the connected user as the receiver
                Timestamp = c.Timestamp,
                Id = c.Id,
                IsRead = c.IsRead,
                LastMessage = c.Content
            })
            .ToList();

        return distinctMessages;
    }
}