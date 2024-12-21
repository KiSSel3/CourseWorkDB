using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Entities;

public class Car : TrackedEntity
{
    public Guid BrandId { get; set; }
    public Guid ModelId { get; set; }
    public Guid BodyTypeId { get; set; }
    public Guid TransmissionTypeId { get; set; }
    public Guid DriveTypeId { get; set; }
    public int Seats { get; set; }
    public int? Mileage { get; set; }

    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n BrandId: {BrandId}," +
               $"\n ModelId: {ModelId}," +
               $"\n BodyTypeId: {BodyTypeId}," +
               $"\n TransmissionTypeId: {TransmissionTypeId}," +
               $"\n DriveTypeId: {DriveTypeId}," +
               $"\n Seats: {Seats}," +
               $"\n Mileage: {Mileage}";
    }
}