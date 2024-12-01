using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;

namespace CarRentPlace.Repositories
{
	public class WheelDriveTypeRepository
	{
		private string _connectionString;

		public WheelDriveTypeRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public WheelDriveTypeModel CreateWheelDriveType(string type)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"INSERT INTO ""wheel_drive_type"" (type) 
                            VALUES (@Type) RETURNING id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Type", type);

						int id = (int)command.ExecuteScalar();

						var newWheelDriveType = new WheelDriveTypeModel { Id = id, Type = type };

						Console.WriteLine($"Log ----> Создан тип привода автомобиля: ID {id}, Тип '{type}'");
						return newWheelDriveType;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при создании типа привода автомобиля: {ex.Message}");
				return null;
			}
		}

		public WheelDriveTypeModel GetWheelDriveTypeById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, type FROM ""wheel_drive_type"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								var wheelDriveType = new WheelDriveTypeModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Type = reader["type"].ToString()
								};

								Console.WriteLine($"Log ----> Получен тип привода автомобиля: ID {wheelDriveType.Id}, Тип '{wheelDriveType.Type}'");
								return wheelDriveType;
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Тип привода автомобиля с ID {id} не найден");
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении типа привода автомобиля с ID {id}: {ex.Message}");
				return null;
			}
		}

		public bool UpdateWheelDriveType(int id, string type)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"UPDATE ""wheel_drive_type"" 
                            SET type = @Type 
                            WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Type", type);
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Обновлен тип привода автомобиля с ID {id}: Тип '{type}'");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Тип привода автомобиля с ID {id} не найден");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при обновлении типа привода автомобиля с ID {id}: {ex.Message}");
				return false;
			}
		}

		public bool DeleteWheelDriveTypeById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"DELETE FROM ""wheel_drive_type"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Удален тип привода автомобиля с ID {id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Тип привода автомобиля с ID {id} не найден");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при удалении типа привода автомобиля с ID {id}: {ex.Message}");
				return false;
			}
		}

		public List<WheelDriveTypeModel> GetAllWheelDriveTypes()
		{
			List<WheelDriveTypeModel> wheelDriveTypes = new List<WheelDriveTypeModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, type FROM ""wheel_drive_type""";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var wheelDriveType = new WheelDriveTypeModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Type = reader["type"].ToString()
								};

								wheelDriveTypes.Add(wheelDriveType);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список всех типов привода автомобилей");
				return wheelDriveTypes;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка типов привода автомобилей: {ex.Message}");
				return null;
			}
		}
	}
}
