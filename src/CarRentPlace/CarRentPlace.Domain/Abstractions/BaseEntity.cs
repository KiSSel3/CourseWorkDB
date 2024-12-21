namespace CarRentPlace.Domain.Abstractions;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.Empty;
    public bool IsDeleted { get; set; } = false;
    
    public override string ToString()
    {
        return $"Id: {Id}," +
               $"\n IsDeleted: {IsDeleted}";
    }
}