namespace CarRentPlace.Domain.Models;

public class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public override string ToString()
    {
        return $" UserId: {UserId}," +
               $"\n RoleId: {RoleId}";
    }
}