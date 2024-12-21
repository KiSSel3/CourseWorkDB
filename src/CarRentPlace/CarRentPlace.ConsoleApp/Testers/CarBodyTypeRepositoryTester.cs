using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.ConsoleApp.Testers;

public class CarBodyTypeRepositoryTester
{
    private readonly ICarBodyTypeRepository _repository;

    public CarBodyTypeRepositoryTester(ICarBodyTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task RunTestsAsync()
    {
        Console.WriteLine("\n==================== TESTING CarBodyTypeRepository ====================\n");
        
        Console.WriteLine("Adding a new car body type...");
        var newCarBodyType = new CarBodyType { Name = "TestBodyType", IsDeleted = false };
        await _repository.AddAsync(newCarBodyType);
        Console.WriteLine("Car body type added successfully.\n");
        
        Console.WriteLine("Fetching car body type by name...");
        var fetchedCarBodyType = await _repository.GetByNameAsync("TestBodyType", includeDeleted: false);
        if (fetchedCarBodyType != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(fetchedCarBodyType);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Car body type not found by name.\n");
        }

        Console.WriteLine("Fetching all car body types...");
        var allCarBodyTypes = await _repository.GetAllAsync(includeDeleted: false);
        foreach (var bodyType in allCarBodyTypes)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(bodyType);
            Console.ResetColor();
        }
        Console.WriteLine();
        
        Console.WriteLine("Updating car body type...");
        if (fetchedCarBodyType != null)
        {
            fetchedCarBodyType.Name = "UpdatedBodyType";
            await _repository.UpdateAsync(fetchedCarBodyType);
            Console.WriteLine("Car body type updated successfully.\n");
        }

        Console.WriteLine("Fetching car body type by Id...");
        if (fetchedCarBodyType != null)
        {
            var carBodyTypeById = await _repository.GetByIdAsync(fetchedCarBodyType.Id, includeDeleted: false);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(carBodyTypeById);
            Console.ResetColor();
        }
        Console.WriteLine();

        Console.WriteLine("Soft deleting car body type...");
        if (fetchedCarBodyType != null)
        {
            await _repository.SoftDeleteAsync(fetchedCarBodyType.Id);
            Console.WriteLine("Car body type soft deleted successfully.\n");
        }
        
        Console.WriteLine("Fetching car body type by Id...");
        if (fetchedCarBodyType != null)
        {
            var carBodyTypeById = await _repository.GetByIdAsync(fetchedCarBodyType.Id, includeDeleted: true);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(carBodyTypeById);
            Console.ResetColor();
        }
        Console.WriteLine();
        
        Console.WriteLine("Deleting car body type...");
        if (fetchedCarBodyType != null)
        {
            await _repository.DeleteAsync(fetchedCarBodyType.Id);
            Console.WriteLine("Car body type deleted successfully.\n");
        }
        
        Console.WriteLine("Fetching all car body types after tests...");
        var finalCarBodyTypes = await _repository.GetAllAsync(includeDeleted: true);
        foreach (var bodyType in finalCarBodyTypes)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(bodyType);
            Console.ResetColor();
        }

        Console.WriteLine("\n==================== TESTING COMPLETE ====================\n");
    }
}