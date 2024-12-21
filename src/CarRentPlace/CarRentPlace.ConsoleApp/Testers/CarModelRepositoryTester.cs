using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.ConsoleApp.Testers;

public class CarModelRepositoryTester
{
    private readonly ICarModelRepository _repository;

    public CarModelRepositoryTester(ICarModelRepository repository)
    {
        _repository = repository;
    }

    public async Task RunTestsAsync()
    {
        Console.WriteLine("\n==================== TESTING CarModelRepository ====================\n");

        // Test AddAsync
        Console.WriteLine("Adding a new car model...");
        var newCarModel = new CarModel { Id = Guid.NewGuid(), Name = "TestModel", IsDeleted = false };
        await _repository.AddAsync(newCarModel);
        Console.WriteLine("Car model added successfully.\n");

        // Test GetByNameAsync
        Console.WriteLine("Fetching car model by name...");
        var fetchedCarModel = await _repository.GetByNameAsync("TestModel", includeDeleted: false);
        if (fetchedCarModel != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(fetchedCarModel);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Car model not found by name.\n");
        }

        // Test GetAllAsync
        Console.WriteLine("Fetching all car models...");
        var allCarModels = await _repository.GetAllAsync(includeDeleted: false);
        foreach (var model in allCarModels)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(model);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test UpdateAsync
        Console.WriteLine("Updating car model...");
        if (fetchedCarModel != null)
        {
            fetchedCarModel.Name = "UpdatedModel";
            await _repository.UpdateAsync(fetchedCarModel);
            Console.WriteLine("Car model updated successfully.\n");
        }

        // Test GetByIdAsync
        Console.WriteLine("Fetching car model by Id...");
        if (fetchedCarModel != null)
        {
            var carModelById = await _repository.GetByIdAsync(fetchedCarModel.Id, includeDeleted: false);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(carModelById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test SoftDeleteAsync
        Console.WriteLine("Soft deleting car model...");
        if (fetchedCarModel != null)
        {
            await _repository.SoftDeleteAsync(fetchedCarModel.Id);
            Console.WriteLine("Car model soft deleted successfully.\n");
        }

        // Test GetByIdAsync with includeDeleted = true
        Console.WriteLine("Fetching car model by Id (include deleted)...");
        if (fetchedCarModel != null)
        {
            var carModelById = await _repository.GetByIdAsync(fetchedCarModel.Id, includeDeleted: true);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(carModelById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test DeleteAsync
        Console.WriteLine("Deleting car model...");
        if (fetchedCarModel != null)
        {
            await _repository.DeleteAsync(fetchedCarModel.Id);
            Console.WriteLine("Car model deleted successfully.\n");
        }

        // Final output of all car models
        Console.WriteLine("Fetching all car models after tests...");
        var finalCarModels = await _repository.GetAllAsync(includeDeleted: true);
        foreach (var model in finalCarModels)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(model);
            Console.ResetColor();
        }

        Console.WriteLine("\n==================== TESTING COMPLETE ====================\n");
    }
}
