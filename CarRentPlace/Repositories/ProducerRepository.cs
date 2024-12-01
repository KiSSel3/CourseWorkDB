using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;

namespace CarRentPlace.Repositories
{
	public class ProducerRepository
	{
		private string _connectionString;

		public ProducerRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public ProducerModel CreateProducer(string country, string name)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"INSERT INTO ""producer"" (country, name) 
                            VALUES (@Country, @Name) RETURNING id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Country", country);
						command.Parameters.AddWithValue("Name", name);

						int id = (int)command.ExecuteScalar();

						var newProducer = new ProducerModel { Id = id, Country = country, Name = name };

						Console.WriteLine($"Log ----> Создан производитель: ID {id}, Название '{name}', Страна '{country}'");
						return newProducer;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при создании производителя: {ex.Message}");
				return null;
			}
		}

		public ProducerModel GetProducerById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, country, name FROM ""producer"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								var producer = new ProducerModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Country = reader["country"].ToString(),
									Name = reader["name"].ToString()
								};

								Console.WriteLine($"Log ----> Получен производитель: ID {producer.Id}, Название '{producer.Name}', Страна '{producer.Country}'");
								return producer;
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Производитель с ID {id} не найден");
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении производителя с ID {id}: {ex.Message}");
				return null;
			}
		}

		public bool UpdateProducer(int id, string country, string name)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"UPDATE ""producer"" 
                            SET country = @Country, name = @Name 
                            WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Country", country);
						command.Parameters.AddWithValue("Name", name);
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Обновлен производитель с ID {id}: Название '{name}', Страна '{country}'");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Производитель с ID {id} не найден");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при обновлении производителя с ID {id}: {ex.Message}");
				return false;
			}
		}

		public bool DeleteProducerById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"DELETE FROM ""producer"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Удален производитель с ID {id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Производитель с ID {id} не найден");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при удалении производителя с ID {id}: {ex.Message}");
				return false;
			}
		}

		public List<ProducerModel> GetAllProducers()
		{
			List<ProducerModel> producers = new List<ProducerModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, country, name FROM ""producer""";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var producer = new ProducerModel
								{
									Id = Convert.ToInt32(reader["id"]),
									Country = reader["country"].ToString(),
									Name = reader["name"].ToString()
								};

								producers.Add(producer);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список всех производителей");
				return producers;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка производителей: {ex.Message}");
				return null;
			}
		}
	}

}
