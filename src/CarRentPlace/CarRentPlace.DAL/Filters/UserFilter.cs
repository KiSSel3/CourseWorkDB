namespace CarRentPlace.DAL.Filters;

public class UserFilter
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IncludeDeleted { get; set; } = false;
}