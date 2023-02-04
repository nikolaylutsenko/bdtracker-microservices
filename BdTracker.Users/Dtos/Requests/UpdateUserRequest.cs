using BdTracker.Shared.Entities;

namespace BdTracker.Users.Dtos.Requests;

public class UpdateUserRequest
{
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public Sex Sex { get; set; }
    public DateOnly Birthday { get; set; }
    public string Occupation { get; set; } = default!;
    public string AboutMe { get; set; } = default!;
}