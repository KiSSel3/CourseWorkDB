using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;

namespace CarRentPlace.Repositories
{
	public class VehicleTypeRepository
	{
		private string _connectionString;

		public VehicleTypeRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public VehicleTypeModel CreateVehicleType(string type)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"INSERT INTO ""vehicle_type"" (type) 
                            VALUES (@Type) RETURNING id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Type", type);

						int id = (int)command.ExecuteScalar();

						var newVehicleType = new VehicleTypeModel { Id = id, Type = type };

						Console.WriteLine($"Log ----> Создан тип транспортного средства: ID {id}, Тип '{type}'");
						return newVehicleType;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при создании типа транспортного средства: {ex.Message}");
				return null;
			}
		}

		public VehicleTypeModel GetVehicleTypeById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, type FROM ""vehicle_type"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								var vehicleType = new VehicleTypeModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Type = reader["type"].ToString()
								};

								Console.WriteLine($"Log ----> Получен тип транспортного средства: ID {vehicleType.Id}, Тип '{vehicleType.Type}'");
								return vehicleType;
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Тип транспортного средства с ID {id} не найден");
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении типа транспортного средства с ID {id}: {ex.Message}");
				return null;
			}
		}

		public bool UpdateVehicleType(int id, string type)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"UPDATE ""vehicle_type"" 
                            SET type = @Type 
                            WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Type", type);
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Обновлен тип транспортного средства с ID {id}: Тип '{type}'");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Тип транспортного средства с ID {id} не найден");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при обновлении типа транспортного средства с ID {id}: {ex.Message}");
				return false;
			}
		}

		public bool DeleteVehicleTypeById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"DELETE FROM ""vehicle_type"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Удален тип транспортного средства с ID {id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Тип транспортного средства с ID {id} не найден");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при удалении типа транспортного средства с ID {id}: {ex.Message}");
				return false;
			}
		}

		public List<VehicleTypeModel> GetAllVehicleTypes()
		{
			List<VehicleTypeModel> vehicleTypes = new List<VehicleTypeModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, type FROM ""vehicle_type""";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var vehicleType = new VehicleTypeModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Type = reader["type"].ToString()
								};

								vehicleTypes.Add(vehicleType);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список всех типов транспортных средств");
				return vehicleTypes;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка типов транспортных средств: {ex.Message}");
				return null;
			}
		}
	}
}
