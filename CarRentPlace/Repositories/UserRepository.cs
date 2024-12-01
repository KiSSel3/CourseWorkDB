using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;

namespace CarRentPlace.Repositories
{
	public class UserRepository
	{
		private string _connectionString;

		public UserRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public UserModel Registration(string login, string password, string fName, string lName, string phoneNumber, string email, int role = 1)
		{
			UserModel user = null;

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var addUserSQL = @"INSERT INTO ""user"" (login, password, first_name, last_name, phone_number, email) 
									   VALUES (@Login, @Password, @FirstName, @LastName, @PhoneNumber, @Email) RETURNING id, login, password, first_name, last_name, phone_number, email;";

					using (var command = new NpgsqlCommand(addUserSQL, connection))
					{
						command.Parameters.AddWithValue("Login", login);
						command.Parameters.AddWithValue("Password", password);
						command.Parameters.AddWithValue("FirstName", fName);
						command.Parameters.AddWithValue("LastName", lName);
						command.Parameters.AddWithValue("PhoneNumber", phoneNumber);
						command.Parameters.AddWithValue("Email", email);

						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								user = new UserModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Login = reader["login"].ToString(),
									Password = reader["password"].ToString(),
									FirstName = reader["first_name"].ToString(),
									LastName = reader["last_name"].ToString(),
									PhoneNumber = reader["phone_number"].ToString(),
									Email = reader["email"].ToString()
								};
							}
						}
					}

					var setUserRoleSQL = @"INSERT INTO ""user_role"" (user_id, role_id) 
										   VALUES (@UserId, @RoleId);";

					using (var command = new NpgsqlCommand(setUserRoleSQL, connection))
					{
						command.Parameters.AddWithValue("UserId", user.Id);
						command.Parameters.AddWithValue("RoleId", role);

						command.ExecuteReader();
					}
				}

				Console.WriteLine("Log ----> Успешная регистрация!");
				return user;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка регистрации!");
				return user;
			}
		}

		public UserModel Login(string login, string password)
		{
			UserModel user = null;

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var selectUserSQL = @"SELECT id, login, password, first_name, last_name, phone_number, email 
										  FROM ""user"" 
										  WHERE login = @Login AND password = @Password";

					using (var command = new NpgsqlCommand(selectUserSQL, connection))
					{
						command.Parameters.AddWithValue("Login", login);
						command.Parameters.AddWithValue("Password", password);

						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								user = new UserModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Login = reader["login"].ToString(),
									Password = reader["password"].ToString(),
									FirstName = reader["first_name"].ToString(),
									LastName = reader["last_name"].ToString(),
									PhoneNumber = reader["phone_number"].ToString(),
									Email = reader["email"].ToString()
								};
							}
						}
					}
				}

				if (user != null)
				{
					Console.WriteLine("Log ----> Успешная авторизация!");
				}
				else
				{
					Console.WriteLine("Log ----> Неудачная попытка авторизации: неверный логин или пароль");
				}

				return user;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка авторизации!:\n{ex.Message}\n\n");
				return user;
			}
		}

		public void DeleteUserById(int userId)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"DELETE FROM ""user"" WHERE id = @UserId";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("UserId", userId);

						command.ExecuteNonQuery();
					}
				}

				Console.WriteLine($"Log ----> Пользователь с ID {userId} успешно удален.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при удалении пользователя с ID {userId}: {ex.Message}");
			}
		}

		public UserModel UpdateUser(int userId, string login, string password, string firstName, string lastName, string phoneNumber, string email)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"UPDATE ""user"" 
                        SET login = @Login, 
                            password = @Password, 
                            first_name = @FirstName, 
                            last_name = @LastName, 
                            phone_number = @PhoneNumber, 
                            email = @Email 
                        WHERE id = @UserId RETURNING id, login, password, first_name, last_name, phone_number, email";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Login", login);
						command.Parameters.AddWithValue("Password", password);
						command.Parameters.AddWithValue("FirstName", firstName);
						command.Parameters.AddWithValue("LastName", lastName);
						command.Parameters.AddWithValue("PhoneNumber", phoneNumber);
						command.Parameters.AddWithValue("Email", email);
						command.Parameters.AddWithValue("UserId", userId);

						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								var updatedUser = new UserModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Login = reader["login"].ToString(),
									Password = reader["password"].ToString(),
									FirstName = reader["first_name"].ToString(),
									LastName = reader["last_name"].ToString(),
									PhoneNumber = reader["phone_number"].ToString(),
									Email = reader["email"].ToString()
								};

								Console.WriteLine($"Log ----> Пользователь с ID {updatedUser.Id} успешно обновлен.");
								return updatedUser;
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Пользователь с ID {userId} не найден.");
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при обновлении пользователя с ID {userId}: {ex.Message}");
				return null;
			}
		}

		public bool IsLoginAvailable(string login)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT COUNT(*) FROM ""user"" WHERE login = @Login";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Login", login);

						var result = (long)command.ExecuteScalar();

						return result == 0;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при проверке доступности логина: {ex.Message}");
				return false;
			}
		}

		public string GetUserRoleById(int userId)
		{
			string userRole = null;

			using (var connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				var sql = @"SELECT r.type FROM ""user"" u
                    JOIN user_role ur ON u.id = ur.user_id
                    JOIN role r ON ur.role_id = r.id
                    WHERE u.id = @UserId";

				using (var command = new NpgsqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("UserId", userId);

					using (var reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							userRole = reader.GetString(reader.GetOrdinal("type"));
						}
					}
				}
			}

			return userRole;
		}
	}
}
