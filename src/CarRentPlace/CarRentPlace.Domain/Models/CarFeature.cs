using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Models;

public class CarFeature
{
    public Guid CarId { get; set; }
    public Guid FeatureId { get; set; }

    public override string ToString()
    {
        return $" CarId: {CarId}," +
               $"\n FeatureId: {FeatureId}";
    }
}