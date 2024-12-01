using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;
using CarRentPlace.Repositories;

namespace CarRentPlace.Services
{
	public class WheelDriveTypeService
	{
		private readonly WheelDriveTypeRepository _wheelDriveTypeRepository;

		public WheelDriveTypeService(string connectionString)
		{
			_wheelDriveTypeRepository = new WheelDriveTypeRepository(connectionString);
		}

		public void ShowWheelDriveTypes()
		{
			List<WheelDriveTypeModel> wheelDriveTypes = _wheelDriveTypeRepository.GetAllWheelDriveTypes();

			if (wheelDriveTypes != null)
			{
				foreach (var wheelDriveType in wheelDriveTypes)
				{
					Console.WriteLine($"ID: {wheelDriveType.Id}, Тип: {wheelDriveType.Type}");
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
					AddWheelDriveType();
					break;
				case "2":
					DeleteWheelDriveType();
					break;
				default:
					Console.WriteLine("Выход");
					break;
			}
		}

		private void AddWheelDriveType()
		{
			Console.WriteLine("Введите тип привода автомобиля:");
			string type = Console.ReadLine();

			var createdWheelDriveType = _wheelDriveTypeRepository.CreateWheelDriveType(type);

			if (createdWheelDriveType != null)
			{
				Console.WriteLine($"Добавлен новый тип привода автомобиля: ID {createdWheelDriveType.Id}, Тип '{createdWheelDriveType.Type}'");
			}
		}

		private void DeleteWheelDriveType()
		{
			Console.WriteLine("Введите ID типа привода автомобиля для удаления:");
			if (int.TryParse(Console.ReadLine(), out int typeId))
			{
				bool deleted = _wheelDriveTypeRepository.DeleteWheelDriveTypeById(typeId);

				if (deleted)
				{
					Console.WriteLine($"Удален тип привода автомобиля с ID {typeId}");
				}
			}
			else
			{
				Console.WriteLine("Неверный формат ID.");
			}
		}
	}

}
