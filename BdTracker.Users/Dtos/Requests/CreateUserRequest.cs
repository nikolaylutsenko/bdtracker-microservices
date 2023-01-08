using System.Text.Json.Serialization;
using BdTracker.Shared.Entities;

namespace BdTracker.Users.Dtos;

public record CreateUserRequest
{
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public Sex Sex { get; set; }
    public DateOnly Birthday { get; set; }
    public string Occupation { get; set; } = default!;
    public string AboutMe { get; set; } = default!;
    public List<string> GroupsIds { get; set; } = new List<string>();
    public Guid WishlistId { get; set; }
}