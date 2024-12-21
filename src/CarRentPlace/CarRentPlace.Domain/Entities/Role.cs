using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{base.ToString()}," +
               $" Name: {Name}";
    }
}