using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;
using CarRentPlace.Repositories;

namespace CarRentPlace.Services
{
	public class OfferService
	{
		private readonly OfferRepository _offerRepository;
		private readonly VehicleRepository _vehicleRepository;

		public int UserId { get; set; } = -1;

		public OfferService(string connectionString)
		{
			_offerRepository = new OfferRepository(connectionString);
			_vehicleRepository = new VehicleRepository(connectionString);
		}

		public void ShowOffers()
		{
			List<OfferModel> offers = _offerRepository.GetOffersByUserId(UserId);

			foreach (var offer in offers)
			{
				Console.WriteLine($"ID: {offer.Id}, ID транспортного средства: {offer.VehicleId}, ID пользователя: {offer.UserId}, " +
					$"Дата публикации: {offer.PublicationDate}, Описание: {offer.Description}, " +
					$"Цена: {offer.Price}, Местоположение: {offer.Location}, Показывать: {offer.Show}, " +
					$"Подтверждено: {offer.Approved}");
			}
		}

		public void ShowOffersWithVehicles()
		{
			List<OfferModel> offers = _offerRepository.GetOffersByUserId(UserId);

			foreach (var offer in offers)
			{
				VehicleModel vehicle = _vehicleRepository.GetVehicleById(offer.VehicleId);

				if (vehicle != null)
				{
					Console.WriteLine($"\n\nID предложения: {offer.Id}, ID транспортного средства: {offer.VehicleId}, ID пользователя: {offer.UserId}, " +
						$"Дата публикации: {offer.PublicationDate}, Описание: {offer.Description}, " +
						$"Цена: {offer.Price}, Местоположение: {offer.Location}, Показывать: {offer.Show}, " +
						$"Подтверждено: {offer.Approved}");

					Console.WriteLine($"Информация о транспортном средстве:");
					Console.WriteLine($"ID: {vehicle.Id}, Производитель: {vehicle.ProducerId}, Тип транспорта: {vehicle.VehicleTypeId}, " +
						$"Привод: {vehicle.WheelDriveTypeId}, Тип трансмиссии: {vehicle.TransmissionTypeId}, Модель: {vehicle.Model}, " +
						$"Пробег: {vehicle.Mileage}, Объем двигателя: {vehicle.EngineDisplacement}, Лошадинные силы: {vehicle.Horsepower}, " +
						$"Год производства: {vehicle.ProductionYear}\n\n");
				}
			}
		}


		public void PerformAction()
		{
			Console.WriteLine("Выберите действие: (1 - Создать, 2 - Удалить, 3 - Обновить, 4 - Скрыть/Показать объявление, 5 - Выйти)");
			string action = Console.ReadLine();

			switch (action)
			{
				case "1":
					CreateOffer();
					break;
				case "2":
					DeleteOffer();
					break;
				case "3":
					UpdateOffer();
					break;
				case "4":
					ToggleOfferVisibility();
					break;
				default:
					Console.WriteLine("Завершение");
					break;
			}
		}

		private void ToggleOfferVisibility()
		{
			ShowOffers();
			Console.WriteLine("Введите ID объявления для скрытия/показа:");
			if (int.TryParse(Console.ReadLine(), out int offerId))
			{
				OfferModel offer = _offerRepository.GetOfferById(offerId);
				if (offer != null)
				{
					bool currentVisibility = offer.Show;
					bool newVisibility = !currentVisibility;

					bool updated = _offerRepository.UpdateOfferShowStatus(offerId, newVisibility);

					if (updated)
					{
						string visibilityAction = newVisibility ? "показано" : "скрыто";
						Console.WriteLine($"Статус объявления с ID {offerId} изменен: теперь {visibilityAction}");
					}
					else
					{
						Console.WriteLine("Не удалось изменить статус показа объявления.");
					}
				}
				else
				{
					Console.WriteLine($"Объявление с ID {offerId} не найдено.");
				}
			}
			else
			{
				Console.WriteLine("Неверный формат ID объявления.");
			}
		}

		private void CreateOffer()
		{
			List<VehicleModel> vehicles = _vehicleRepository.GetVehiclesByUserId(UserId);

			foreach (var vehicle in vehicles)
			{
				Console.WriteLine($"ID: {vehicle.Id}, Производитель: {vehicle.ProducerId}, Тип транспорта: {vehicle.VehicleTypeId}, " +
					$"Привод: {vehicle.WheelDriveTypeId}, Тип трансмиссии: {vehicle.TransmissionTypeId}, Модель: {vehicle.Model}, " +
					$"Пробег: {vehicle.Mileage}, Объем двигателя: {vehicle.EngineDisplacement}, Лошадинные силы: {vehicle.Horsepower}, " +
					$"Год производства: {vehicle.ProductionYear}");
			}

			Console.WriteLine("Введите ID транспортного средства:");
			int vehicleId = int.Parse(Console.ReadLine());

			Console.WriteLine("Введите описание:");
			string description = Console.ReadLine();

			Console.WriteLine("Введите цену:");
			decimal price = decimal.Parse(Console.ReadLine());

			Console.WriteLine("Введите местоположение:");
			string location = Console.ReadLine();

			_offerRepository.CreateOffer(vehicleId, UserId, DateTime.Now, description, price, location, true, false);
		}

		private void DeleteOffer()
		{
			ShowOffers();
			Console.WriteLine("Введите ID предложения для удаления:");
			if (int.TryParse(Console.ReadLine(), out int offerId))
			{
				_offerRepository.DeleteOffer(offerId);
			}
			else
			{
				Console.WriteLine("Неверный формат ID.");
			}
		}

		private void UpdateOffer()
		{
			ShowOffers();
			Console.WriteLine("Введите ID предложения для обновления:");
			if (int.TryParse(Console.ReadLine(), out int offerId))
			{
				OfferModel offer = _offerRepository.GetOfferById(offerId);
				if (offer != null)
				{
					Console.WriteLine("Введите описание:");
					offer.Description = Console.ReadLine();

					Console.WriteLine("Введите цену:");
					offer.Price = decimal.Parse(Console.ReadLine());

					Console.WriteLine("Введите местоположение:");
					offer.Location = Console.ReadLine();


					_offerRepository.UpdateOffer(offer);
				}
				else
				{
					Console.WriteLine($"Предложение с ID {offerId} не найдено");
				}
			}
			else
			{
				Console.WriteLine("Неверный формат ID.");
			}
		}

		public void ShowUnapprovedOffers()
		{
			List<OfferModel> unapprovedOffers = _offerRepository.GetUnapprovedOffers();

			if (unapprovedOffers != null && unapprovedOffers.Count > 0)
			{
				foreach (var offer in unapprovedOffers)
				{
					Console.WriteLine($"ID: {offer.Id}, ID транспортного средства: {offer.VehicleId}, ID пользователя: {offer.UserId}, " +
						$"Дата публикации: {offer.PublicationDate}, Описание: {offer.Description}, " +
						$"Цена: {offer.Price}, Местоположение: {offer.Location}, Показывать: {offer.Show}, " +
						$"Подтверждено: {offer.Approved}");
				}

				Console.WriteLine("Введите ID предложения, чтобы подтвердить: ");
				if (int.TryParse(Console.ReadLine(), out int offerId))
				{
					var offerToUpdate = unapprovedOffers.FirstOrDefault(o => o.Id == offerId);
					if (offerToUpdate != null)
					{
						bool updated = _offerRepository.UpdateOfferApprovedStatus(offerId, true);
						if (updated)
						{
							Console.WriteLine($"Подтверждение для предложения с ID {offerId} выполнено.");
						}
						else
						{
							Console.WriteLine($"Не удалось подтвердить предложение с ID {offerId}.");
						}
					}
					else
					{
						Console.WriteLine($"Предложение с ID {offerId} не найдено.");
					}
				}
				else
				{
					Console.WriteLine("Некорректный формат ID предложения.");
				}
			}
			else
			{
				Console.WriteLine("Нет неподтвержденных предложений.");
			}
		}

		public void ShowApprovedOffers()
		{
			List<OfferModel> approvedOffers = _offerRepository.GetApprovedOffers();

			if (approvedOffers != null && approvedOffers.Count > 0)
			{
				foreach (var offer in approvedOffers)
				{
					Console.WriteLine($"ID: {offer.Id}, ID транспортного средства: {offer.VehicleId}, ID пользователя: {offer.UserId}, " +
						$"Дата публикации: {offer.PublicationDate}, Описание: {offer.Description}, " +
						$"Цена: {offer.Price}, Местоположение: {offer.Location}, Показывать: {offer.Show}, " +
						$"Подтверждено: {offer.Approved}");

					VehicleModel vehicle = _vehicleRepository.GetVehicleById(offer.VehicleId);
					if (vehicle != null)
					{
						Console.WriteLine($"Транспортное средство: ID: {vehicle.Id}, Производитель: {vehicle.ProducerId}, " +
							$"Тип транспорта: {vehicle.VehicleTypeId}, Привод: {vehicle.WheelDriveTypeId}, " +
							$"Тип трансмиссии: {vehicle.TransmissionTypeId}, Модель: {vehicle.Model}, " +
							$"Пробег: {vehicle.Mileage}, Объем двигателя: {vehicle.EngineDisplacement}, " +
							$"Лошадинные силы: {vehicle.Horsepower}, Год производства: {vehicle.ProductionYear}");
					}
				}
			}
			else
			{
				Console.WriteLine("Нет предложений.");
			}
		}


	}

}
