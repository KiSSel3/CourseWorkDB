using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.ConsoleApp.Testers;

public class FeatureRepositoryTester
{
    private readonly IFeatureRepository _repository;

    public FeatureRepositoryTester(IFeatureRepository repository)
    {
        _repository = repository;
    }

    public async Task RunTestsAsync()
    {
        Console.WriteLine("\n==================== TESTING FeatureRepository ====================\n");

        // Test AddAsync
        Console.WriteLine("Adding a new feature...");
        var newFeature = new Feature { Id = Guid.NewGuid(), Name = "TestFeature", IsDeleted = false };
        await _repository.AddAsync(newFeature);
        Console.WriteLine("Feature added successfully.\n");

        // Test GetByNameAsync
        Console.WriteLine("Fetching feature by name...");
        var fetchedFeature = await _repository.GetByNameAsync("TestFeature", includeDeleted: false);
        if (fetchedFeature != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(fetchedFeature);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Feature not found by name.\n");
        }

        // Test GetAllAsync
        Console.WriteLine("Fetching all features...");
        var allFeatures = await _repository.GetAllAsync(includeDeleted: false);
        foreach (var feature in allFeatures)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(feature);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test UpdateAsync
        Console.WriteLine("Updating feature...");
        if (fetchedFeature != null)
        {
            fetchedFeature.Name = "UpdatedFeature";
            await _repository.UpdateAsync(fetchedFeature);
            Console.WriteLine("Feature updated successfully.\n");
        }

        // Test GetByIdAsync
        Console.WriteLine("Fetching feature by Id...");
        if (fetchedFeature != null)
        {
            var featureById = await _repository.GetByIdAsync(fetchedFeature.Id, includeDeleted: false);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(featureById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test SoftDeleteAsync
        Console.WriteLine("Soft deleting feature...");
        if (fetchedFeature != null)
        {
            await _repository.SoftDeleteAsync(fetchedFeature.Id);
            Console.WriteLine("Feature soft deleted successfully.\n");
        }

        // Test GetByIdAsync with includeDeleted = true
        Console.WriteLine("Fetching feature by Id (include deleted)...");
        if (fetchedFeature != null)
        {
            var featureById = await _repository.GetByIdAsync(fetchedFeature.Id, includeDeleted: true);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(featureById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test DeleteAsync
        Console.WriteLine("Deleting feature...");
        if (fetchedFeature != null)
        {
            await _repository.DeleteAsync(fetchedFeature.Id);
            Console.WriteLine("Feature deleted successfully.\n");
        }

        // Final output of all features
        Console.WriteLine("Fetching all features after tests...");
        var finalFeatures = await _repository.GetAllAsync(includeDeleted: true);
        foreach (var feature in finalFeatures)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(feature);
            Console.ResetColor();
        }

        Console.WriteLine("\n==================== TESTING COMPLETE ====================\n");
    }
}
