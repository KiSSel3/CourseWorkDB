using CarRentPlace.DAL.Repositories.Interfaces;
using DriveType = CarRentPlace.Domain.Entities.DriveType;

namespace CarRentPlace.ConsoleApp.Testers;

public class DriveTypeRepositoryTester
{
    private readonly IDriveTypeRepository _repository;

    public DriveTypeRepositoryTester(IDriveTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task RunTestsAsync()
    {
        Console.WriteLine("\n==================== TESTING DriveTypeRepository ====================\n");

        // Test AddAsync
        Console.WriteLine("Adding a new drive type...");
        var newDriveType = new DriveType { Id = Guid.NewGuid(), Name = "TestDriveType", IsDeleted = false };
        await _repository.AddAsync(newDriveType);
        Console.WriteLine("Drive type added successfully.\n");

        // Test GetByNameAsync
        Console.WriteLine("Fetching drive type by name...");
        var fetchedDriveType = await _repository.GetByNameAsync("TestDriveType", includeDeleted: false);
        if (fetchedDriveType != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(fetchedDriveType);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Drive type not found by name.\n");
        }

        // Test GetAllAsync
        Console.WriteLine("Fetching all drive types...");
        var allDriveTypes = await _repository.GetAllAsync(includeDeleted: false);
        foreach (var driveType in allDriveTypes)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(driveType);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test UpdateAsync
        Console.WriteLine("Updating drive type...");
        if (fetchedDriveType != null)
        {
            fetchedDriveType.Name = "UpdatedDriveType";
            await _repository.UpdateAsync(fetchedDriveType);
            Console.WriteLine("Drive type updated successfully.\n");
        }

        // Test GetByIdAsync
        Console.WriteLine("Fetching drive type by Id...");
        if (fetchedDriveType != null)
        {
            var driveTypeById = await _repository.GetByIdAsync(fetchedDriveType.Id, includeDeleted: false);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(driveTypeById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test SoftDeleteAsync
        Console.WriteLine("Soft deleting drive type...");
        if (fetchedDriveType != null)
        {
            await _repository.SoftDeleteAsync(fetchedDriveType.Id);
            Console.WriteLine("Drive type soft deleted successfully.\n");
        }

        // Test GetByIdAsync with includeDeleted = true
        Console.WriteLine("Fetching drive type by Id (include deleted)...");
        if (fetchedDriveType != null)
        {
            var driveTypeById = await _repository.GetByIdAsync(fetchedDriveType.Id, includeDeleted: true);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(driveTypeById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test DeleteAsync
        Console.WriteLine("Deleting drive type...");
        if (fetchedDriveType != null)
        {
            await _repository.DeleteAsync(fetchedDriveType.Id);
            Console.WriteLine("Drive type deleted successfully.\n");
        }

        // Final output of all drive types
        Console.WriteLine("Fetching all drive types after tests...");
        var finalDriveTypes = await _repository.GetAllAsync(includeDeleted: true);
        foreach (var driveType in finalDriveTypes)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(driveType);
            Console.ResetColor();
        }

        Console.WriteLine("\n==================== TESTING COMPLETE ====================\n");
    }
}