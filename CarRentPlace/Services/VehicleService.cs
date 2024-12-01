using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;
using CarRentPlace.Repositories;

namespace CarRentPlace.Services
{
	public class VehicleService
	{
		private readonly VehicleRepository _vehicleRepository;

		private readonly ProducerService _producerService;
		private readonly TransmissionTypeService _transmissionTypeService;
		private readonly VehicleTypeService _vehicleTypeService;
		private readonly WheelDriveTypeService _wheelDriveTypeService;

		public int UserId { get; set; } = -1;

		public VehicleService(string connectionString)
		{
			_vehicleRepository = new VehicleRepository(connectionString);
			_producerService = new ProducerService(connectionString);
			_transmissionTypeService = new TransmissionTypeService(connectionString);
			_vehicleTypeService = new VehicleTypeService(connectionString);
			_wheelDriveTypeService = new WheelDriveTypeService(connectionString);
		}

		public void ShowVehicles()
		{
			List<VehicleModel> vehicles = _vehicleRepository.GetVehiclesByUserId(UserId);

			foreach (var vehicle in vehicles)
			{
				Console.WriteLine($"ID: {vehicle.Id}, Производитель: {vehicle.ProducerId}, Тип транспорта: {vehicle.VehicleTypeId}, " +
					$"Привод: {vehicle.WheelDriveTypeId}, Тип трансмиссии: {vehicle.TransmissionTypeId}, Модель: {vehicle.Model}, " +
					$"Пробег: {vehicle.Mileage}, Объем двигателя: {vehicle.EngineDisplacement}, Лошадинные силы: {vehicle.Horsepower}, " +
					$"Год производства: {vehicle.ProductionYear}");
			}
		}

		public void PerformAction()
		{
			Console.WriteLine("Выберите действие: (1 - Добавить, 2 - Удалить, 3 - Обновить, 4 - Выйти)");
			string action = Console.ReadLine();

			switch (action)
			{
				case "1":
					AddVehicle();
					break;
				case "2":
					DeleteVehicle();
					break;
				case "3":
					UpdateVehicle();
					break;
				default:
					Console.WriteLine("Выход");
					break;
			}
		}

		private void AddVehicle()
		{
			_producerService.ShowProducers();
			Console.WriteLine("Введите ID производителя:");
			int producerId = int.Parse(Console.ReadLine());

			_vehicleTypeService.ShowVehicleTypes();
			Console.WriteLine("Введите ID типа транспорта:");
			int vehicleTypeId = int.Parse(Console.ReadLine());

			_wheelDriveTypeService.ShowWheelDriveTypes();
			Console.WriteLine("Введите ID привода:");
			int wheelDriveTypeId = int.Parse(Console.ReadLine());

			_transmissionTypeService.ShowTransmissionTypes();
			Console.WriteLine("Введите ID типа трансмиссии:");
			int transmissionTypeId = int.Parse(Console.ReadLine());

			Console.WriteLine("Введите модель:");
			string model = Console.ReadLine();

			Console.WriteLine("Введите пробег:");
			decimal mileage = decimal.Parse(Console.ReadLine());

			Console.WriteLine("Введите объем двигателя:");
			decimal engineDisplacement = decimal.Parse(Console.ReadLine());

			Console.WriteLine("Введите количество лошадиных сил:");
			int horsepower = int.Parse(Console.ReadLine());

			Console.WriteLine("Введите год производства:");
			int year;
			int.TryParse(Console.ReadLine(), out year);
			DateTime productionYear = new DateTime(year, 1, 1);


			_vehicleRepository.CreateVehicle(producerId, vehicleTypeId, wheelDriveTypeId, transmissionTypeId, UserId,
				model, mileage, engineDisplacement, horsepower, productionYear);
		}

		private void DeleteVehicle()
		{
			Console.WriteLine("Введите ID транспортного средства для удаления:");
			if (int.TryParse(Console.ReadLine(), out int vehicleId))
			{
				_vehicleRepository.DeleteVehicle(vehicleId);
			}
			else
			{
				Console.WriteLine("Неверный формат ID.");
			}
		}

		private void UpdateVehicle()
		{
			Console.WriteLine("Введите ID транспортного средства для обновления:");
			if (int.TryParse(Console.ReadLine(), out int vehicleId))
			{
				VehicleModel vehicle = _vehicleRepository.GetVehicleById(vehicleId);
				if (vehicle != null)
				{
					_producerService.ShowProducers();
					Console.WriteLine("Введите ID производителя:");
					int producerId = int.Parse(Console.ReadLine());

					_vehicleTypeService.ShowVehicleTypes();
					Console.WriteLine("Введите ID типа транспорта:");
					int vehicleTypeId = int.Parse(Console.ReadLine());

					_wheelDriveTypeService.ShowWheelDriveTypes();
					Console.WriteLine("Введите ID привода:");
					int wheelDriveTypeId = int.Parse(Console.ReadLine());

					_transmissionTypeService.ShowTransmissionTypes();
					Console.WriteLine("Введите ID типа трансмиссии:");
					int transmissionTypeId = int.Parse(Console.ReadLine());

					Console.WriteLine("Введите модель:");
					string model = Console.ReadLine();

					Console.WriteLine("Введите пробег:");
					decimal mileage = decimal.Parse(Console.ReadLine());

					Console.WriteLine("Введите объем двигателя:");
					decimal engineDisplacement = decimal.Parse(Console.ReadLine());

					Console.WriteLine("Введите количество лошадиных сил:");
					int horsepower = int.Parse(Console.ReadLine());

					Console.WriteLine("Введите год производства:");
					DateTime productionYear = DateTime.Parse(Console.ReadLine());

					vehicle.ProducerId = producerId;
					vehicle.VehicleTypeId = vehicleTypeId;
					vehicle.WheelDriveTypeId = wheelDriveTypeId;
					vehicle.TransmissionTypeId = transmissionTypeId;
					vehicle.Model = model;
					vehicle.Mileage = mileage;
					vehicle.EngineDisplacement = engineDisplacement;
					vehicle.Horsepower = horsepower;
					vehicle.ProductionYear = productionYear;

					_vehicleRepository.UpdateVehicle(vehicle);
				}
				else
				{
					Console.WriteLine($"Транспортное средство с ID {vehicleId} не найдено");
				}
			}
			else
			{
				Console.WriteLine("Неверный формат ID.");
			}
		}

	}

}
