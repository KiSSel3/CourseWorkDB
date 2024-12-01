using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;

namespace CarRentPlace.Repositories
{
	public class TransmissionTypeRepository
	{
		private string _connectionString;

		public TransmissionTypeRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public TransmissionTypeModel CreateTransmissionType(string type)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"INSERT INTO ""transmission_type"" (type) 
                            VALUES (@Type) RETURNING id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Type", type);

						int id = (int)command.ExecuteScalar();

						var newTransmissionType = new TransmissionTypeModel { Id = id, Type = type };

						Console.WriteLine($"Log ----> Создан тип трансмиссии: ID {id}, Тип '{type}'");
						return newTransmissionType;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при создании типа трансмиссии: {ex.Message}");
				return null;
			}
		}

		public TransmissionTypeModel GetTransmissionTypeById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, type FROM ""transmission_type"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								var transmissionType = new TransmissionTypeModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Type = reader["type"].ToString()
								};

								Console.WriteLine($"Log ----> Получен тип трансмиссии: ID {transmissionType.Id}, Тип '{transmissionType.Type}'");
								return transmissionType;
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Тип трансмиссии с ID {id} не найден");
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении типа трансмиссии с ID {id}: {ex.Message}");
				return null;
			}
		}

		public bool UpdateTransmissionType(int id, string type)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"UPDATE ""transmission_type"" 
                            SET type = @Type 
                            WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Type", type);
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Обновлен тип трансмиссии с ID {id}: Тип '{type}'");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Тип трансмиссии с ID {id} не найден");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при обновлении типа трансмиссии с ID {id}: {ex.Message}");
				return false;
			}
		}

		public bool DeleteTransmissionTypeById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"DELETE FROM ""transmission_type"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Удален тип трансмиссии с ID {id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Тип трансмиссии с ID {id} не найден");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при удалении типа трансмиссии с ID {id}: {ex.Message}");
				return false;
			}
		}

		public List<TransmissionTypeModel> GetAllTransmissionTypes()
		{
			List<TransmissionTypeModel> transmissionTypes = new List<TransmissionTypeModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, type FROM ""transmission_type""";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var transmissionType = new TransmissionTypeModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Type = reader["type"].ToString()
								};

								transmissionTypes.Add(transmissionType);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список всех типов трансмиссии");
				return transmissionTypes;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка типов трансмиссии: {ex.Message}");
				return null;
			}
		}
	}
}
