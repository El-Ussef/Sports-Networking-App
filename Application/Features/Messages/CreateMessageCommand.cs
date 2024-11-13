using Application.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Features.Messages;

public class CreateMessageCommand : IRequest<int>
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Content { get; set; }
}
//If needed 
public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMessageService _messageService;

    public CreateMessageCommandHandler(IApplicationDbContext context, IMessageService messageService)
    {
        _context = context;
        _messageService = messageService;
    }

    public async Task<int> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Content = request.Content,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        // Notify the receiver about the new message
        await _messageService.CreateMessageAsync(request.SenderId, request.ReceiverId, request.Content);

        return message.Id;
    }
}