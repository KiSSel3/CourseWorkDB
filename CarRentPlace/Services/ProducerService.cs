using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;
using CarRentPlace.Repositories;

namespace CarRentPlace.Services
{
	public class ProducerService
	{
		private readonly ProducerRepository _producerRepository;

		public ProducerService(string connectionString)
		{
			_producerRepository = new ProducerRepository(connectionString);
		}

		public void ShowProducers()
		{
			List<ProducerModel> producers = _producerRepository.GetAllProducers();

			foreach (var producer in producers)
			{
				Console.WriteLine($"ID: {producer.Id}, Название: {producer.Name}, Страна: {producer.Country}");
			}
		}

		public void PerformAction()
		{
			Console.WriteLine("Выберите действие: (1 - Добавить, 2 - Удалить, 3 - Выйти)");
			string action = Console.ReadLine();

			switch (action)
			{
				case "1":
					AddProducer();
					break;
				case "2":
					DeleteProducer();
					break;
				default:
					Console.WriteLine("Выход");
					break;
			}
		}

		private void AddProducer()
		{
			Console.WriteLine("Введите название производителя:");
			string name = Console.ReadLine();

			Console.WriteLine("Введите страну производителя:");
			string country = Console.ReadLine();

			_producerRepository.CreateProducer(name, country);
		}

		private void DeleteProducer()
		{
			Console.WriteLine("Введите ID производителя для удаления:");
			if (int.TryParse(Console.ReadLine(), out int producerId))
			{
				_producerRepository.DeleteProducerById(producerId);
			}
			else
			{
				Console.WriteLine("Неверный формат ID.");
			}
		}
	}

}
