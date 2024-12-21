using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Entities;

public class CarBrand : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n Name: {Name}," +
               $"\n Country: {Country}";
    }
}