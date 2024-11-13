using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class UserConnection
{
    
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ConnectionId { get; set; }
}