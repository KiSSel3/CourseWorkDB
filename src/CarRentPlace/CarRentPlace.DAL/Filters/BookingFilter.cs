using CarRentPlace.Domain.Enums;

namespace CarRentPlace.DAL.Filters;

public class BookingFilter : PaginationFilter
{
    public Guid? UserId { get; set; }
    public Guid? RentOfferId { get; set; }
    public BookingStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IncludeDeleted { get; set; } = false;
}