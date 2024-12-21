using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.ConsoleApp.Testers;

public class TransmissionTypeRepositoryTester
{
    private readonly ITransmissionTypeRepository _repository;

    public TransmissionTypeRepositoryTester(ITransmissionTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task RunTestsAsync()
    {
        Console.WriteLine("\n==================== TESTING TransmissionTypeRepository ====================\n");

        // Test AddAsync
        Console.WriteLine("Adding a new transmission type...");
        var newTransmissionType = new TransmissionType { Id = Guid.NewGuid(), Name = "TestTransmission", IsDeleted = false };
        await _repository.AddAsync(newTransmissionType);
        Console.WriteLine("Transmission type added successfully.\n");

        // Test GetByNameAsync
        Console.WriteLine("Fetching transmission type by name...");
        var fetchedTransmissionType = await _repository.GetByNameAsync("TestTransmission", includeDeleted: false);
        if (fetchedTransmissionType != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(fetchedTransmissionType);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Transmission type not found by name.\n");
        }

        // Test GetAllAsync
        Console.WriteLine("Fetching all transmission types...");
        var allTransmissionTypes = await _repository.GetAllAsync(includeDeleted: false);
        foreach (var transmissionType in allTransmissionTypes)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(transmissionType);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test UpdateAsync
        Console.WriteLine("Updating transmission type...");
        if (fetchedTransmissionType != null)
        {
            fetchedTransmissionType.Name = "UpdatedTransmission";
            await _repository.UpdateAsync(fetchedTransmissionType);
            Console.WriteLine("Transmission type updated successfully.\n");
        }

        // Test GetByIdAsync
        Console.WriteLine("Fetching transmission type by Id...");
        if (fetchedTransmissionType != null)
        {
            var transmissionTypeById = await _repository.GetByIdAsync(fetchedTransmissionType.Id, includeDeleted: false);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(transmissionTypeById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test SoftDeleteAsync
        Console.WriteLine("Soft deleting transmission type...");
        if (fetchedTransmissionType != null)
        {
            await _repository.SoftDeleteAsync(fetchedTransmissionType.Id);
            Console.WriteLine("Transmission type soft deleted successfully.\n");
        }

        // Test GetByIdAsync with includeDeleted = true
        Console.WriteLine("Fetching transmission type by Id (include deleted)...");
        if (fetchedTransmissionType != null)
        {
            var transmissionTypeById = await _repository.GetByIdAsync(fetchedTransmissionType.Id, includeDeleted: true);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(transmissionTypeById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test DeleteAsync
        Console.WriteLine("Deleting transmission type...");
        if (fetchedTransmissionType != null)
        {
            await _repository.DeleteAsync(fetchedTransmissionType.Id);
            Console.WriteLine("Transmission type deleted successfully.\n");
        }

        // Final output of all transmission types
        Console.WriteLine("Fetching all transmission types after tests...");
        var finalTransmissionTypes = await _repository.GetAllAsync(includeDeleted: true);
        foreach (var transmissionType in finalTransmissionTypes)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(transmissionType);
            Console.ResetColor();
        }

        Console.WriteLine("\n==================== TESTING COMPLETE ====================\n");
    }
}
