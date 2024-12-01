using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;

namespace CarRentPlace.Repositories
{
	public class WheelRepository
	{
		private string _connectionString;

		public WheelRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public WheelModel CreateWheel(decimal treadWidth, decimal profileHeight, decimal radius)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"INSERT INTO ""wheel"" (tread_width, profile_height, radius) 
                            VALUES (@TreadWidth, @ProfileHeight, @Radius) RETURNING id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("TreadWidth", treadWidth);
						command.Parameters.AddWithValue("ProfileHeight", profileHeight);
						command.Parameters.AddWithValue("Radius", radius);

						int id = (int)command.ExecuteScalar();

						var newWheel = new WheelModel
						{
							Id = id,
							TreadWidth = treadWidth,
							ProfileHeight = profileHeight,
							Radius = radius
						};

						Console.WriteLine($"Log ----> Создано колесо: ID {id}");
						return newWheel;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при создании колеса: {ex.Message}");
				return null;
			}
		}

		public WheelModel GetWheelById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, tread_width, profile_height, radius FROM ""wheel"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								var wheel = new WheelModel
								{
									Id = Convert.ToInt32(reader["id"]),
									TreadWidth = Convert.ToDecimal(reader["tread_width"]),
									ProfileHeight = Convert.ToDecimal(reader["profile_height"]),
									Radius = Convert.ToDecimal(reader["radius"])
								};

								Console.WriteLine($"Log ----> Получено колесо: ID {wheel.Id}");
								return wheel;
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Колесо с ID {id} не найдено");
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении колеса с ID {id}: {ex.Message}");
				return null;
			}
		}

		public bool UpdateWheel(int id, decimal treadWidth, decimal profileHeight, decimal radius)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"UPDATE ""wheel"" 
                            SET tread_width = @TreadWidth, profile_height = @ProfileHeight, radius = @Radius 
                            WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("TreadWidth", treadWidth);
						command.Parameters.AddWithValue("ProfileHeight", profileHeight);
						command.Parameters.AddWithValue("Radius", radius);
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Обновлено колесо: ID {id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Колесо с ID {id} не найдено");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при обновлении колеса с ID {id}: {ex.Message}");
				return false;
			}
		}

		public bool DeleteWheelById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"DELETE FROM ""wheel"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Удалено колесо: ID {id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Колесо с ID {id} не найдено");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при удалении колеса с ID {id}: {ex.Message}");
				return false;
			}
		}

		public List<WheelModel> GetAllWheels()
		{
			List<WheelModel> wheels = new List<WheelModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, tread_width, profile_height, radius FROM ""wheel""";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var wheel = new WheelModel
								{
									Id = Convert.ToInt32(reader["id"]),
									TreadWidth = Convert.ToDecimal(reader["tread_width"]),
									ProfileHeight = Convert.ToDecimal(reader["profile_height"]),
									Radius = Convert.ToDecimal(reader["radius"])
								};

								wheels.Add(wheel);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список всех колес");
				return wheels;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка колес: {ex.Message}");
				return null;
			}
		}
	}
}
