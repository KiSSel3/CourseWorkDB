using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Entities;

public class RentOfferImage : TrackedEntity
{
    public Guid RentOfferId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n RentOfferId: {RentOfferId}," +
               $"\n ImageUrl: {ImageUrl}";
    }
}