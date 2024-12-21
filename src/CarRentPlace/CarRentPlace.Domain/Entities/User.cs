using CarRentPlace.Domain.Abstractions;

namespace CarRentPlace.Domain.Entities;

public class User : TrackedEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{base.ToString()}," +
               $"\n FirstName: {FirstName}," +
               $"\n LastName: {LastName}," +
               $"\n Email: {Email}," +
               $"\n PhoneNumber: {PhoneNumber}," +
               $"\n PasswordHash: {PasswordHash}";
    }
}