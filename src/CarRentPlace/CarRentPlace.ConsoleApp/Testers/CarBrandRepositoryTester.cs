using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.ConsoleApp.Testers;

public class CarBrandRepositoryTester
{
    private readonly ICarBrandRepository _repository;

    public CarBrandRepositoryTester(ICarBrandRepository repository)
    {
        _repository = repository;
    }

    public async Task RunTestsAsync()
    {
        Console.WriteLine("\n==================== TESTING CarBrandRepository ====================\n");

        // Test AddAsync
        Console.WriteLine("Adding a new car brand...");
        var newCarBrand = new CarBrand { Id = Guid.NewGuid(), Name = "TestBrand", Country = "TestCountry", IsDeleted = false };
        await _repository.AddAsync(newCarBrand);
        Console.WriteLine("Car brand added successfully.\n");

        // Test GetByNameAsync
        Console.WriteLine("Fetching car brand by name...");
        var fetchedCarBrand = await _repository.GetByNameAsync("TestBrand", includeDeleted: false);
        if (fetchedCarBrand != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(fetchedCarBrand);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Car brand not found by name.\n");
        }

        // Test GetAllAsync
        Console.WriteLine("Fetching all car brands...");
        var allCarBrands = await _repository.GetAllAsync(includeDeleted: false);
        foreach (var brand in allCarBrands)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(brand);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test UpdateAsync
        Console.WriteLine("Updating car brand...");
        if (fetchedCarBrand != null)
        {
            fetchedCarBrand.Name = "UpdatedBrand";
            fetchedCarBrand.Country = "UpdatedCountry";
            await _repository.UpdateAsync(fetchedCarBrand);
            Console.WriteLine("Car brand updated successfully.\n");
        }

        // Test GetByIdAsync
        Console.WriteLine("Fetching car brand by Id...");
        if (fetchedCarBrand != null)
        {
            var carBrandById = await _repository.GetByIdAsync(fetchedCarBrand.Id, includeDeleted: false);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(carBrandById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test SoftDeleteAsync
        Console.WriteLine("Soft deleting car brand...");
        if (fetchedCarBrand != null)
        {
            await _repository.SoftDeleteAsync(fetchedCarBrand.Id);
            Console.WriteLine("Car brand soft deleted successfully.\n");
        }

        // Test GetByIdAsync with includeDeleted = true
        Console.WriteLine("Fetching car brand by Id (include deleted)...");
        if (fetchedCarBrand != null)
        {
            var carBrandById = await _repository.GetByIdAsync(fetchedCarBrand.Id, includeDeleted: true);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(carBrandById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test DeleteAsync
        Console.WriteLine("Deleting car brand...");
        if (fetchedCarBrand != null)
        {
            await _repository.DeleteAsync(fetchedCarBrand.Id);
            Console.WriteLine("Car brand deleted successfully.\n");
        }

        // Final output of all car brands
        Console.WriteLine("Fetching all car brands after tests...");
        var finalCarBrands = await _repository.GetAllAsync(includeDeleted: true);
        foreach (var brand in finalCarBrands)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(brand);
            Console.ResetColor();
        }

        Console.WriteLine("\n==================== TESTING COMPLETE ====================\n");
    }
}