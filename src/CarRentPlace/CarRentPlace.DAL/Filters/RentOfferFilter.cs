namespace CarRentPlace.DAL.Filters;

public class RentOfferFilter : PaginationFilter
{
    public Guid? UserId { get; set; }
    public Guid? CarId { get; set; }
    public decimal? MinPricePerDay { get; set; }
    public decimal? MaxPricePerDay { get; set; }
    public string? Location { get; set; }
    public bool IsAvailable { get; set; } = true;
    public bool IncludeDeleted { get; set; } = false;
}
