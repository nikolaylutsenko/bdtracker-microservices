using BdTracker.Shared.Entities;

namespace BdTracker.Users.Dtos.Requests;

public record CreateUserRequest
{
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public Sex Sex { get; set; }
    public DateTime Birthday { get; set; }
    public string Occupation { get; set; } = default!;
    public string AboutMe { get; set; } = default!;
    public List<Guid> GroupsIds { get; set; } = new List<Guid>();
    public Guid WishlistId { get; set; }
}