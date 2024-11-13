using Application.Contracts;
using Domain.Entities;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

// [Authorize]
public class MessagesController: BaseApiController
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMessageService _messageService;

    public MessagesController(
        ICurrentUserService currentUserService,
        IMessageService messageService
        )
    {
        _currentUserService = currentUserService;
        _messageService = messageService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<MessageVM>>> GetMessages( int receiverId, int pageNumber = 1, int pageSize = 20)
    {
        var currentUser = _currentUserService;
        if (currentUser is { IsAuthenticated: true })
        {
            int senderId = int.Parse(currentUser.Id);
            var messages = await _messageService.GetMessagesAsync(senderId, receiverId, pageNumber, pageSize);
            return Ok(messages);
        }

        return BadRequest();
    }
    
    [HttpGet("all")]
    [Authorize]
    public async Task<ActionResult<List<MessageOverViewVM>>> GetAllMessages(int pageNumber = 1, int pageSize = 20)
    {
        var currentUser = _currentUserService;
        if (currentUser is { IsAuthenticated: true })
        {
            int userId = int.Parse(currentUser.Id);
            var messages = await _messageService.GetAllMessagesAsync(userId, pageNumber, pageSize);
            return Ok(messages);
        }

        return BadRequest();
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<int>> PostMessage(int receiverId, string content)
    {
        var currentUser = _currentUserService;
        if (currentUser is { IsAuthenticated: true })
        {
            int senderId = int.Parse(currentUser.Id);
            var messageId = await _messageService.CreateMessageAsync(senderId, receiverId, content);
            return Ok(messageId);
        }
        return BadRequest();
    }
}