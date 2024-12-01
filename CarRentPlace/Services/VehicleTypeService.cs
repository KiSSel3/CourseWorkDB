using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;
using CarRentPlace.Repositories;

namespace CarRentPlace.Services
{
	public class VehicleTypeService
	{
		private readonly VehicleTypeRepository _vehicleTypeRepository;

		public VehicleTypeService(string connectionString)
		{
			_vehicleTypeRepository = new VehicleTypeRepository(connectionString);
		}

		public void ShowVehicleTypes()
		{
			List<VehicleTypeModel> vehicleTypes = _vehicleTypeRepository.GetAllVehicleTypes();

			if (vehicleTypes != null)
			{
				foreach (var vehicleType in vehicleTypes)
				{
					Console.WriteLine($"ID: {vehicleType.Id}, Тип: {vehicleType.Type}");
				}
			}
		}

		public void PerformAction()
		{
			Console.WriteLine("Выберите действие: (1 - Добавить, 2 - Удалить, 3 - Выйти)");
			string action = Console.ReadLine();

			switch (action)
			{
				case "1":
					AddVehicleType();
					break;
				case "2":
					DeleteVehicleType();
					break;
				default:
					Console.WriteLine("Выход");
					break;
			}
		}

		private void AddVehicleType()
		{
			Console.WriteLine("Введите тип транспортного средства:");
			string type = Console.ReadLine();

			var createdVehicleType = _vehicleTypeRepository.CreateVehicleType(type);

			if (createdVehicleType != null)
			{
				Console.WriteLine($"Добавлен новый тип транспортного средства: ID {createdVehicleType.Id}, Тип '{createdVehicleType.Type}'");
			}
		}

		private void DeleteVehicleType()
		{
			Console.WriteLine("Введите ID типа транспортного средства для удаления:");
			if (int.TryParse(Console.ReadLine(), out int typeId))
			{
				bool deleted = _vehicleTypeRepository.DeleteVehicleTypeById(typeId);

				if (deleted)
				{
					Console.WriteLine($"Удален тип транспортного средства с ID {typeId}");
				}
			}
			else
			{
				Console.WriteLine("Неверный формат ID.");
			}
		}
	}

}
