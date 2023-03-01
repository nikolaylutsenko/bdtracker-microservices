using BdTracker.Shared.Entities;

namespace BdTracker.Groups.Entities;

public class Group : BaseEntity
{
    public string Name { get; set; } = default!;
}