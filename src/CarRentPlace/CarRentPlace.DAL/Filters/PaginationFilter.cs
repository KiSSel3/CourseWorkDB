namespace CarRentPlace.DAL.Filters;

public class PaginationFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = int.MaxValue;
}