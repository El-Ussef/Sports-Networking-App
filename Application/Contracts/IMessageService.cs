using Domain.Entities;
using Infrastructure.Dtos;

namespace Application.Contracts;

public interface IMessageService
{
    Task<int> CreateMessageAsync(int senderId, int receiverId, string content);
    Task<List<MessageVM>> GetMessagesAsync(int senderId, int receiverId, int pageNumber, int pageSize);
    Task<List<MessageOverViewVM>> GetAllMessagesAsync(int userId, int pageNumber, int pageSize);
}