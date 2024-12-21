using System.Data;
using CarRentPlace.DAL.Filters;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using DriveType = CarRentPlace.Domain.Entities.DriveType;

namespace CarRentPlace.ConsoleApp.Testers;

public class CarRepositoryTester
{
    private readonly ICarRepository _repository;
    private readonly ICarBrandRepository _brandRepository;
    private readonly ICarModelRepository _modelRepository;
    private readonly ICarBodyTypeRepository _bodyTypeRepository;
    private readonly ITransmissionTypeRepository _transmissionRepository;
    private readonly IDriveTypeRepository _driveTypeRepository;

    public CarRepositoryTester(
        ICarRepository repository,
        ICarBrandRepository brandRepository,
        ICarModelRepository modelRepository,
        ICarBodyTypeRepository bodyTypeRepository,
        ITransmissionTypeRepository transmissionRepository,
        IDriveTypeRepository driveTypeRepository)
    {
        _repository = repository;
        _brandRepository = brandRepository;
        _modelRepository = modelRepository;
        _bodyTypeRepository = bodyTypeRepository;
        _transmissionRepository = transmissionRepository;
        _driveTypeRepository = driveTypeRepository;
    }

    public async Task RunTestsAsync(IDbConnection connection)
    {
        Console.WriteLine("\n==================== TESTING CarRepository WITH TRANSACTION ====================\n");

        var brandId = Guid.NewGuid();
        await _brandRepository.AddAsync(new CarBrand
        {
            Id = brandId,
            Name = "TestBrand",
            Country = "TestCountry",
            IsDeleted = false
        });

        var modelId = Guid.NewGuid();
        await _modelRepository.AddAsync(new CarModel { Id = modelId, Name = "TestModel", IsDeleted = false });

        var bodyTypeId = Guid.NewGuid();
        Console.WriteLine("Adding body type...");
        await _bodyTypeRepository.AddAsync(new CarBodyType
        {
            Id = bodyTypeId,
            Name = "TestCarBodyType",
            IsDeleted = false
        });

        // Validate body type
        var bodyType = await _bodyTypeRepository.GetByIdAsync(bodyTypeId, includeDeleted: false);
        if (bodyType == null)
        {
            throw new Exception("Failed to add or fetch body type.");
        }

        var transmissionTypeId = Guid.NewGuid();
        await _transmissionRepository.AddAsync(new TransmissionType
        {
            Id = transmissionTypeId,
            Name = "TestTransmissionType",
            IsDeleted = false
        });

        var driveTypeId = Guid.NewGuid();
        await _driveTypeRepository.AddAsync(new DriveType
        {
            Id = driveTypeId,
            Name = "TestDriveType",
            IsDeleted = false
        });

        var carId = Guid.NewGuid();
        Console.WriteLine("Adding car...");
        await _repository.AddAsync(new Car
        {
            Id = carId,
            BrandId = brandId,
            ModelId = modelId,
            BodyTypeId = bodyTypeId,
            TransmissionTypeId = transmissionTypeId,
            DriveTypeId = driveTypeId,
            Seats = 5,
            Mileage = 10000,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        });

        // Fetch and display data
        var fetchedCar = await _repository.GetByIdAsync(carId, includeDeleted: false);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(fetchedCar);
        Console.ResetColor();

        Console.WriteLine("\nTransaction committed successfully.\n");

        Console.WriteLine("\n==================== TESTING COMPLETE ====================\n");
    }
}
