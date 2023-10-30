using Domain.Common;

namespace Domain.Entities;

public class ManagerDocument : AbstractDocument
{
    public Manager Manager { get; set; }
}