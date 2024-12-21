using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Entities;

public class TransmissionType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n Name: {Name}";
    }
}