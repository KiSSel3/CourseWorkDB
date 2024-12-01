using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;
using CarRentPlace.Repositories;

namespace CarRentPlace.Services
{
	public class TransmissionTypeService
	{
		private readonly TransmissionTypeRepository _transmissionTypeRepository;

		public TransmissionTypeService(string connectionString)
		{
			_transmissionTypeRepository = new TransmissionTypeRepository(connectionString);
		}

		public void ShowTransmissionTypes()
		{
			List<TransmissionTypeModel> transmissionTypes = _transmissionTypeRepository.GetAllTransmissionTypes();

			if (transmissionTypes != null)
			{
				foreach (var transmissionType in transmissionTypes)
				{
					Console.WriteLine($"ID: {transmissionType.Id}, Тип: {transmissionType.Type}");
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
					AddTransmissionType();
					break;
				case "2":
					DeleteTransmissionType();
					break;
				default:
					Console.WriteLine("Выход");
					break;
			}
		}

		private void AddTransmissionType()
		{
			Console.WriteLine("Введите тип трансмиссии:");
			string type = Console.ReadLine();

			var createdTransmissionType = _transmissionTypeRepository.CreateTransmissionType(type);

			if (createdTransmissionType != null)
			{
				Console.WriteLine($"Добавлен новый тип трансмиссии: ID {createdTransmissionType.Id}, Тип '{createdTransmissionType.Type}'");
			}
		}

		private void DeleteTransmissionType()
		{
			Console.WriteLine("Введите ID типа трансмиссии для удаления:");
			if (int.TryParse(Console.ReadLine(), out int typeId))
			{
				bool deleted = _transmissionTypeRepository.DeleteTransmissionTypeById(typeId);

				if (deleted)
				{
					Console.WriteLine($"Удален тип трансмиссии с ID {typeId}");
				}
			}
			else
			{
				Console.WriteLine("Неверный формат ID.");
			}
		}
	}

}
