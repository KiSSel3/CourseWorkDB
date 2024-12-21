using CarRentPlace.Domain.Abstractions;
using CarRentPlace.Domain.Enums;

namespace CarRentPlace.Domain.Entities;

public class Booking : TrackedEntity
{
    public Guid UserId { get; set; }
    public Guid RentOfferId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n UserId: {UserId}," +
               $"\n RentOfferId: {RentOfferId}," +
               $"\n StartDate: {StartDate}," +
               $"\n EndDate: {EndDate}," +
               $"\n TotalPrice: {TotalPrice}," +
               $"\n Status: {Status}";
    }
}
