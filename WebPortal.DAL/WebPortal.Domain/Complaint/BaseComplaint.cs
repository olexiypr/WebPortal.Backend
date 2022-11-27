namespace WebPortal.Domain.Complaint;

public class BaseComplaint : BaseEntity
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
}