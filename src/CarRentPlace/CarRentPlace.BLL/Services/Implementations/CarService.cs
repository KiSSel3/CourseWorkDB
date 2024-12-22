using CarRentPlace.BLL.Helpers;
using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.BLL.ViewModels.Car;
using CarRentPlace.DAL.Filters;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using CarRentPlace.Domain.Models;

namespace CarRentPlace.BLL.Services.Implementations;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IFeatureRepository _featureRepository;
    private readonly ICarFeatureRepository _carFeatureRepository;

    public CarService(
        ICarRepository carRepository,
        IFeatureRepository featureRepository,
        ICarFeatureRepository carFeatureRepository)
    {
        _carRepository = carRepository;
        _featureRepository = featureRepository;
        _carFeatureRepository = carFeatureRepository;
    }

    public async Task<PagedList<Car>> GetByFilterAsync(CarFilter filter, CancellationToken cancellationToken = default)
    {
        var cars = await _carRepository.GetByFilterAsync(filter, cancellationToken);
        var totalCount = await _carRepository.GetCountByFilterAsync(filter, cancellationToken);

        return new PagedList<Car>(
            cars.ToList(),
            totalCount,
            filter.PageNumber,
            filter.PageSize
        );
    }

    public async Task<Car> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var car = await _carRepository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (car == null)
        {
            throw new KeyNotFoundException($"Car with ID '{id}' not found.");
        }

        return car;
    }

    public async Task CreateAsync(CreateCarViewModel model, CancellationToken cancellationToken = default)
    {
        var newCar = new Car
        {
            Id = Guid.NewGuid(),
            BrandId = model.BrandId,
            ModelId = model.ModelId,
            BodyTypeId = model.BodyTypeId,
            TransmissionTypeId = model.TransmissionTypeId,
            DriveTypeId = model.DriveTypeId,
            Seats = model.Seats,
            Mileage = model.Mileage,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _carRepository.AddAsync(newCar, cancellationToken);
    }

    public async Task UpdateAsync(UpdateCarViewModel model, CancellationToken cancellationToken = default)
    {
        var existingCar = await _carRepository.GetByIdAsync(model.Id, includeDeleted: false, cancellationToken);
        if (existingCar == null)
        {
            throw new KeyNotFoundException($"Car with ID '{model.Id}' not found.");
        }

        existingCar.BrandId = model.BrandId;
        existingCar.ModelId = model.ModelId;
        existingCar.BodyTypeId = model.BodyTypeId;
        existingCar.TransmissionTypeId = model.TransmissionTypeId;
        existingCar.DriveTypeId = model.DriveTypeId;
        existingCar.Seats = model.Seats;
        existingCar.Mileage = model.Mileage;
        existingCar.UpdatedAt = DateTime.UtcNow;

        await _carRepository.UpdateAsync(existingCar, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var car = await _carRepository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (car == null)
        {
            throw new KeyNotFoundException($"Car with ID '{id}' not found.");
        }

        await _carRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task AddFeatureToCarAsync(Guid carId, string featureName, CancellationToken cancellationToken = default)
    {
        var car = await _carRepository.GetByIdAsync(carId, includeDeleted: false, cancellationToken);
        if (car == null)
        {
            throw new KeyNotFoundException($"Car with ID '{carId}' not found.");
        }
        
        var feature = await _featureRepository.GetByNameAsync(featureName, includeDeleted: false, cancellationToken);
        if (feature == null)
        {
            throw new KeyNotFoundException($"Feature with name '{featureName}' not found.");
        }

        var existingLink = await _carFeatureRepository.GetByCarIdAsync(carId, includeDeleted: false, cancellationToken);
        if (existingLink.Any(cf => cf.FeatureId == feature.Id))
        {
            throw new InvalidOperationException($"Feature '{featureName}' is already linked to car '{carId}'.");
        }

        var carFeature = new CarFeature
        {
            CarId = carId,
            FeatureId = feature.Id
        };
        await _carFeatureRepository.AddAsync(carFeature, cancellationToken);
    }

    public async Task RemoveFeatureFromCarAsync(Guid carId, string featureName, CancellationToken cancellationToken = default)
    {
        var car = await _carRepository.GetByIdAsync(carId, includeDeleted: false, cancellationToken);
        if (car == null)
        {
            throw new KeyNotFoundException($"Car with ID '{carId}' not found.");
        }
        
        var feature = await _featureRepository.GetByNameAsync(featureName, includeDeleted: false, cancellationToken);
        if (feature == null)
        {
            throw new KeyNotFoundException($"Feature with name '{featureName}' not found.");
        }
        
        await _carFeatureRepository.DeleteAsync(carId, feature.Id, cancellationToken);
    }
}
