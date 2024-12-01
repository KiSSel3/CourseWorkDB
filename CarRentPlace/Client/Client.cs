using CarRentPlace.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;
using CarRentPlace.Services;

namespace CarRentPlace.Client
{
	public class Client
	{
		private readonly UserService _userService;
		private readonly ProducerService _producerService;
		private readonly TransmissionTypeService _transmissionTypeService;
		private readonly VehicleTypeService _vehicleTypeService;
		private readonly WheelDriveTypeService _wheelDriveTypeService;
		private readonly VehicleService _vehicleService;
		private readonly OfferService _offerService;

		private UserModel curUser = null;
		public Client(string connectionString)
		{
			_userService = new UserService(connectionString);
			_producerService = new ProducerService(connectionString);
			_transmissionTypeService = new TransmissionTypeService(connectionString);
			_vehicleTypeService = new VehicleTypeService(connectionString);
			_wheelDriveTypeService = new WheelDriveTypeService(connectionString);
			_vehicleService = new VehicleService(connectionString);
			_offerService = new OfferService(connectionString);
		}

		private void Authorization()
		{
			while (true)
			{
				Console.WriteLine();

				Console.WriteLine("Выберите действие:");
				Console.WriteLine("1. Зарегистрироваться");
				Console.WriteLine("2. Войти в аккаунт");

				Console.WriteLine();

				string choice = Console.ReadLine();

				switch (choice)
				{
					case "1":
						curUser = _userService.RegisterUser();
						return;
					case "2":
						curUser = _userService.LoginUser();
						return;
					default:
						Console.WriteLine("\nНекорректный выбор.\n");
						break;
				}

				Console.WriteLine();
			}
		}

		public void StartClient()
		{
			_offerService.ShowApprovedOffers();

			while (true)
			{
				if (curUser == null)
				{
					Authorization();
				}

				Console.WriteLine("\nВыберите действие:");
				Console.WriteLine("1. Посмотреть информацию об аккаунте");
				Console.WriteLine("2. Изменить личные данные");
				Console.WriteLine("3. Выйти из аккаунта");
				Console.WriteLine("4. Удалить профиль");
				Console.WriteLine("5. Посмотреть список всех объявлений");
				Console.WriteLine("6. Посмотреть список своих объявлений");
				Console.WriteLine("7. Изменить объявление");
				Console.WriteLine("8. Показать ТС");
				Console.WriteLine("9. Изменить ТС");

				if (_userService.IsAdmin(curUser.Id))
				{
					Console.WriteLine("10. Открыть админ панель");
				}

				string choice = Console.ReadLine();

				switch (choice)
				{
					case "1":
						ShowUserInfo();
						break;
					case "2":
						curUser = _userService.UpdateUser(curUser);
						break;
					case "3":
						curUser = null;
						break;
					case "4":
						_userService.DeleteUser(curUser.Id);
						curUser = null;						
						break;
					case "5":
						_offerService.UserId = curUser.Id;
						_offerService.ShowApprovedOffers();
						break;
					case "6":
						_offerService.UserId = curUser.Id;
						_offerService.ShowOffersWithVehicles();
						break;
					case "7":
						_offerService.UserId = curUser.Id;
						_offerService.PerformAction();
						break;
					case "8":
						_vehicleService.UserId = curUser.Id;
						_vehicleService.ShowVehicles();
						break;
					case "9":
						_vehicleService.UserId = curUser.Id;
						_vehicleService.PerformAction();
						break;
					case "10":
						if (!_userService.IsAdmin(curUser.Id))
						{
							Console.WriteLine("\nНекорректный выбор.\n");
							break;
						}
						AdminPanel();
						break;
					default:
						Console.WriteLine("\nНекорректный выбор.\n");
						break;
				}

				Console.WriteLine();
			}
		}

		private void AdminPanel()
		{
			while (true)
			{
				Console.WriteLine("\nВыберите действие:");
				Console.WriteLine("1. Изменить Producer");
				Console.WriteLine("2. Изменить TransmissionType");
				Console.WriteLine("3. Изменить VehicleType");
				Console.WriteLine("4. Изменить WheelDriveType");
				Console.WriteLine("5. Модерация объявлений");
				Console.WriteLine("6. Выход");

				string choice = Console.ReadLine();

				switch (choice)
				{
					case "1":
						_producerService.ShowProducers();
						_producerService.PerformAction();
						break;
					case "2":
						_transmissionTypeService.ShowTransmissionTypes();
						_transmissionTypeService.PerformAction();
						break;
					case "3":
						_vehicleTypeService.ShowVehicleTypes();
						_vehicleTypeService.PerformAction();
						break;
					case "4":
						_wheelDriveTypeService.ShowWheelDriveTypes();
						_wheelDriveTypeService.PerformAction();
						break;
					case "5":
						_offerService.ShowUnapprovedOffers();
						break;
					case "6":
						return;
					default:
						Console.WriteLine("\nНекорректный выбор.\n");
						break;
				}

				Console.WriteLine();
			}
		}

		private void ShowUserInfo()
		{
			Console.WriteLine("\nИнформация о пользователе:");
			Console.WriteLine($"ID: {curUser.Id}");
			Console.WriteLine($"Логин: {curUser.Login}");
			Console.WriteLine($"Имя: {curUser.FirstName}");
			Console.WriteLine($"Фамилия: {curUser.LastName}");
			Console.WriteLine($"Номер телефона: {curUser.PhoneNumber}");
			Console.WriteLine($"Email: {curUser.Email}\n");
		}
	}

}
