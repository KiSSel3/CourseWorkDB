using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Entities;

public class DriveType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n Name: {Name}";
    }
}
