namespace CarRentPlace.Domain.Abstractions;

public abstract class TrackedEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n CreatedAt: {CreatedAt}," +
               $"\n UpdatedAt: {UpdatedAt}";
    }
}