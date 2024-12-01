using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;

namespace CarRentPlace.Repositories
{
	public class UserHistoryRepository
	{
		private string _connectionString;

		public UserHistoryRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void AddUserHistory(int userId, int operationId, DateTime dateTime)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"INSERT INTO ""user_history"" (user_id, operation_id, datetime) 
                            VALUES (@UserId, @OperationId, @DateTime)";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("UserId", userId);
						command.Parameters.AddWithValue("OperationId", operationId);
						command.Parameters.AddWithValue("DateTime", dateTime);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Запись истории для пользователя ID {userId} добавлена");
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при добавлении записи истории для пользователя: {ex.Message}");
			}
		}

		public List<UserHistoryModel> GetUserHistoryByUserId(int userId)
		{
			List<UserHistoryModel> userHistory = new List<UserHistoryModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, user_id, operation_id, datetime FROM ""user_history"" WHERE user_id = @UserId";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("UserId", userId);

						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var history = new UserHistoryModel
								{
									Id = Convert.ToInt32(reader["id"]),
									UserId = Convert.ToInt32(reader["user_id"]),
									OperationId = Convert.ToInt32(reader["operation_id"]),
									DateTime = Convert.ToDateTime(reader["datetime"])
								};

								userHistory.Add(history);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получена история для пользователя ID {userId}");
				return userHistory;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении истории для пользователя: {ex.Message}");
				return null;
			}
		}
	}
}
