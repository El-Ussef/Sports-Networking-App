namespace Infrastructure.Dtos;

public class MessageListVM
{
    public int MessageCount { get; set; }

    public List<MessageOverViewVM> MessageOverView { get; set; } = new List<MessageOverViewVM>();
}

public class MessageVM
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string Sender { get; set; }
    public int ReceiverId { get; set; }
    public string Receiver { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; }
}

public class MessageOverViewVM
{
    public int Id { get; set; }
    public string? SenderPicturePath { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    
    public string Sender { get; set; } = string.Empty;
    public string LastMessage { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; }
}
