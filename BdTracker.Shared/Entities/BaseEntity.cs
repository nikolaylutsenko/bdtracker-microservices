namespace BdTracker.Shared.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; } = default!;
    public string? EditedBy { get; set; }
    public bool IsActive { get; set; }
}
