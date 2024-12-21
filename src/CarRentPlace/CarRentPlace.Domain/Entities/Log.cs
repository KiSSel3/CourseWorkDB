using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Entities;

public class Log : BaseEntity
{
    public Guid? UserId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n UserId: {UserId}," +
               $"\n ActionType: {ActionType}," +
               $"\n EntityType: {EntityType}," +
               $"\n EntityId: {EntityId}," +
               $"\n OldValues: {OldValues}," +
               $"\n NewValues: {NewValues}," +
               $"\n CreatedAt: {CreatedAt}";
    }
}