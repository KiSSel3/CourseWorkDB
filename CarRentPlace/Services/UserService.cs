using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;
using CarRentPlace.Repositories;

namespace CarRentPlace.Services
{
	public class UserService
	{
		private readonly UserRepository _userRepo;

		public UserService(string connectionString)
		{
			_userRepo = new UserRepository(connectionString);
		}

		public UserModel RegisterUser()
		{
			try
			{
				Console.WriteLine("Введите данные для регистрации:");
				string login, password, firstName, lastName, phoneNumber, email;

				while (true)
				{
					Console.Write("Логин: ");
					login = Console.ReadLine();

					if (!_userRepo.IsLoginAvailable(login))
					{
						Console.WriteLine("Этот логин уже используется. Попробуйте другой.");
					}
					else
					{
						break;
					}
				}

				Console.Write("Пароль: ");
				password = Console.ReadLine();

				Console.Write("Имя: ");
				firstName = Console.ReadLine();

				Console.Write("Фамилия: ");
				lastName = Console.ReadLine();

				Console.Write("Номер телефона: ");
				phoneNumber = Console.ReadLine();

				Console.Write("Email: ");
				email = Console.ReadLine();

				return _userRepo.Registration(login, password, firstName, lastName, phoneNumber, email);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при регистрации пользователя: {ex.Message}");
				return null;
			}
		}

		public UserModel LoginUser()
		{
			try
			{
				Console.WriteLine("Введите данные для входа:");
				string login, password;

				while (true)
				{
					Console.Write("Логин: ");
					login = Console.ReadLine();

					Console.Write("Пароль: ");
					password = Console.ReadLine();

					var user = _userRepo.Login(login, password);

					if (user != null)
					{
						return user;
					}
					else
					{
						Console.WriteLine("Неверный логин или пароль. Попробуйте снова.");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при входе пользователя: {ex.Message}");
				return null;
			}
		}

		public UserModel UpdateUser(UserModel user)
		{
			while (true)
			{
				try
				{
					Console.WriteLine("Введите новые данные:");

					Console.Write("Логин: ");
					string newLogin = Console.ReadLine();

					Console.Write("Пароль: ");
					string newPassword = Console.ReadLine();

					Console.Write("Имя: ");
					string newFirstName = Console.ReadLine();

					Console.Write("Фамилия: ");
					string newLastName = Console.ReadLine();

					Console.Write("Номер телефона: ");
					string newPhoneNumber = Console.ReadLine();

					Console.Write("Email: ");
					string newEmail = Console.ReadLine();

					var updateUser = _userRepo.UpdateUser(user.Id, newLogin, newPassword, newFirstName, newLastName, newPhoneNumber, newEmail);

					if (updateUser != null)
					{
						return updateUser;
					}
					
					Console.WriteLine("Ошибка при обновлении информации пользователя. Пожалуйста, попробуйте снова.");					
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Ошибка при обновлении информации пользователя: {ex.Message}");
					return null;
				}
			}
		}

		public void DeleteUser(int userId)
		{
			Console.WriteLine("Вы уверены, что хотите удалить аккаунт? (yes/no)");
			string confirmation = Console.ReadLine();

			if (confirmation.ToLower() == "yes")
			{
				_userRepo.DeleteUserById(userId);
			}
			else
			{
				Console.WriteLine("Отмена удаления аккаунта.");
			}
		}

		public bool IsAdmin(int userId)
		{
			return _userRepo.GetUserRoleById(userId) == "admin";
		}
	}
}
