using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentPlace.Repositories
{
	public class SuitableWheelsRepository
	{
		private string _connectionString;

		public SuitableWheelsRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public bool AttachWheelToVehicle(int vehicleId, int wheelId)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"INSERT INTO ""suitable_wheels"" (vehicle_id, wheel_id) 
                            VALUES (@VehicleId, @WheelId)";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("VehicleId", vehicleId);
						command.Parameters.AddWithValue("WheelId", wheelId);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Колесо ID {wheelId} прикреплено к транспортному средству ID {vehicleId}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Не удалось прикрепить колесо ID {wheelId} к транспортному средству ID {vehicleId}");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при прикреплении колеса к транспортному средству: {ex.Message}");
				return false;
			}
		}

		public bool DetachWheelFromVehicle(int vehicleId, int wheelId)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"DELETE FROM ""suitable_wheels"" WHERE vehicle_id = @VehicleId AND wheel_id = @WheelId";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("VehicleId", vehicleId);
						command.Parameters.AddWithValue("WheelId", wheelId);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Колесо ID {wheelId} откреплено от транспортного средства ID {vehicleId}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Не удалось открепить колесо ID {wheelId} от транспортного средства ID {vehicleId}");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при откреплении колеса от транспортного средства: {ex.Message}");
				return false;
			}
		}
	}
}
