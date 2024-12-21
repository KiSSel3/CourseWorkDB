using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Entities;

public class CarBodyType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n Name: {Name}";
    }
}