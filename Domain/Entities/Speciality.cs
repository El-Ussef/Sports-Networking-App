using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Speciality : BaseEntity
{
    public string Label { get; set; } = string.Empty;

    public UserType UserType { get; set; }
}