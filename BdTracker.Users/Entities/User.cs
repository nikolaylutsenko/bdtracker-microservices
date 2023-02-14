using System;
using BdTracker.Shared.Entities;

namespace BdTracker.Users.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public Sex Sex { get; set; }
    public DateTime Birthday { get; set; }
    public string Occupation { get; set; } = default!;
    public string AboutMe { get; set; } = default!;
    public List<string> GroupsIds { get; set; } = new List<string>();
    public Guid WishlistId { get; set; }
}