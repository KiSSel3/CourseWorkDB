using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Entities;

public class RentOffer : TrackedEntity
{
    public Guid UserId { get; set; }
    public Guid CarId { get; set; }
    public decimal PricePerDay { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsAvailable { get; set; }

    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n UserId: {UserId}," +
               $"\n CarId: {CarId}," +
               $"\n PricePerDay: {PricePerDay}," +
               $"\n Location: {Location}," +
               $"\n IsAvailable: {IsAvailable}," +
               $"\n Description: {Description}";
    }
}