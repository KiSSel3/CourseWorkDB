namespace CarRentPlace.DAL.Filters;

public class CarFilter : PaginationFilter
{
    public Guid? BrandId { get; set; }
    public Guid? ModelId { get; set; }
    public Guid? BodyTypeId { get; set; }
    public Guid? TransmissionTypeId { get; set; }
    public Guid? DriveTypeId { get; set; }
    public int? MinSeats { get; set; }
    public int? MaxSeats { get; set; }
    public int? MinMileage { get; set; }
    public int? MaxMileage { get; set; }
    public bool IncludeDeleted { get; set; } = false;
}
