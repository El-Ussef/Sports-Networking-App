using Domain.Common;

namespace Domain.Entities;

public class MedicalStaffDocument : AbstractDocument
{
    public MedicalStaff MedicalStaff { get; set; }
}