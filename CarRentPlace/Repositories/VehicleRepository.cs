using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentPlace.Models;

namespace CarRentPlace.Repositories
{
	public class VehicleRepository
	{
		private string _connectionString;

		public VehicleRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public VehicleModel CreateVehicle(int producerId, int vehicleTypeId, int wheelDriveTypeId, int transmissionTypeId, int userId,
	string model, decimal mileage, decimal engineDisplacement, int horsepower, DateTime productionYear)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"INSERT INTO ""vehicle"" (producer_id, vehicle_type_id, wheel_drive_type_id, transmission_type_id, 
                    user_id, model, mileage, engine_displacement, horsepower, production_year) 
                    VALUES (@ProducerId, @VehicleTypeId, @WheelDriveTypeId, @TransmissionTypeId, @UserId, @Model, @Mileage, 
                    @EngineDisplacement, @Horsepower, @ProductionYear) RETURNING id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("ProducerId", producerId);
						command.Parameters.AddWithValue("VehicleTypeId", vehicleTypeId);
						command.Parameters.AddWithValue("WheelDriveTypeId", wheelDriveTypeId);
						command.Parameters.AddWithValue("TransmissionTypeId", transmissionTypeId);
						command.Parameters.AddWithValue("UserId", userId);
						command.Parameters.AddWithValue("Model", model);
						command.Parameters.AddWithValue("Mileage", mileage);
						command.Parameters.AddWithValue("EngineDisplacement", engineDisplacement);
						command.Parameters.AddWithValue("Horsepower", horsepower);
						command.Parameters.AddWithValue("ProductionYear", productionYear);

						int id = (int)command.ExecuteScalar();

						var newVehicle = new VehicleModel
						{
							Id = id,
							ProducerId = producerId,
							VehicleTypeId = vehicleTypeId,
							WheelDriveTypeId = wheelDriveTypeId,
							TransmissionTypeId = transmissionTypeId,
							UserId = userId,
							Model = model,
							Mileage = mileage,
							EngineDisplacement = engineDisplacement,
							Horsepower = horsepower,
							ProductionYear = productionYear
						};

						Console.WriteLine($"Log ----> Создано транспортное средство: ID {id}");
						return newVehicle;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при создании транспортного средства: {ex.Message}");
				return null;
			}
		}

		public VehicleModel GetVehicleById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT producer_id, vehicle_type_id, wheel_drive_type_id, transmission_type_id, model, 
                    mileage, engine_displacement, horsepower, production_year, user_id FROM ""vehicle"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								var vehicle = new VehicleModel
								{
									Id = id,
									ProducerId = Convert.ToInt32(reader["producer_id"]),
									VehicleTypeId = Convert.ToInt32(reader["vehicle_type_id"]),
									WheelDriveTypeId = Convert.ToInt32(reader["wheel_drive_type_id"]),
									TransmissionTypeId = Convert.ToInt32(reader["transmission_type_id"]),
									Model = reader["model"].ToString(),
									Mileage = Convert.ToDecimal(reader["mileage"]),
									EngineDisplacement = Convert.ToDecimal(reader["engine_displacement"]),
									Horsepower = Convert.ToInt32(reader["horsepower"]),
									ProductionYear = Convert.ToDateTime(reader["production_year"]),
									UserId = Convert.ToInt32(reader["user_id"]) // Добавлено поле UserId
								};

								Console.WriteLine($"Log ----> Получено транспортное средство: ID {id}");
								return vehicle;
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Транспортное средство с ID {id} не найдено");
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении транспортного средства с ID {id}: {ex.Message}");
				return null;
			}
		}

		public List<VehicleModel> GetAllVehicles()
		{
			List<VehicleModel> vehicles = new List<VehicleModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, producer_id, vehicle_type_id, wheel_drive_type_id, transmission_type_id, model, 
                    mileage, engine_displacement, horsepower, production_year, user_id FROM ""vehicle""";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var vehicle = new VehicleModel
								{
									Id = Convert.ToInt32(reader["id"]),
									ProducerId = Convert.ToInt32(reader["producer_id"]),
									VehicleTypeId = Convert.ToInt32(reader["vehicle_type_id"]),
									WheelDriveTypeId = Convert.ToInt32(reader["wheel_drive_type_id"]),
									TransmissionTypeId = Convert.ToInt32(reader["transmission_type_id"]),
									Model = reader["model"].ToString(),
									Mileage = Convert.ToDecimal(reader["mileage"]),
									EngineDisplacement = Convert.ToDecimal(reader["engine_displacement"]),
									Horsepower = Convert.ToInt32(reader["horsepower"]),
									ProductionYear = Convert.ToDateTime(reader["production_year"]),
									UserId = Convert.ToInt32(reader["user_id"])
								};

								vehicles.Add(vehicle);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список всех транспортных средств");
				return vehicles;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка транспортных средств: {ex.Message}");
				return null;
			}
		}

		public bool UpdateVehicle(VehicleModel vehicle)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"UPDATE ""vehicle"" SET producer_id = @ProducerId, vehicle_type_id = @VehicleTypeId, 
                    wheel_drive_type_id = @WheelDriveTypeId, transmission_type_id = @TransmissionTypeId, 
                    model = @Model, mileage = @Mileage, engine_displacement = @EngineDisplacement, 
                    horsepower = @Horsepower, production_year = @ProductionYear, user_id = @UserId WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", vehicle.Id);
						command.Parameters.AddWithValue("ProducerId", vehicle.ProducerId);
						command.Parameters.AddWithValue("VehicleTypeId", vehicle.VehicleTypeId);
						command.Parameters.AddWithValue("WheelDriveTypeId", vehicle.WheelDriveTypeId);
						command.Parameters.AddWithValue("TransmissionTypeId", vehicle.TransmissionTypeId);
						command.Parameters.AddWithValue("Model", vehicle.Model);
						command.Parameters.AddWithValue("Mileage", vehicle.Mileage);
						command.Parameters.AddWithValue("EngineDisplacement", vehicle.EngineDisplacement);
						command.Parameters.AddWithValue("Horsepower", vehicle.Horsepower);
						command.Parameters.AddWithValue("ProductionYear", vehicle.ProductionYear);
						command.Parameters.AddWithValue("UserId", vehicle.UserId);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Обновлено транспортное средство: ID {vehicle.Id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Транспортное средство с ID {vehicle.Id} не обновлено");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при обновлении транспортного средства: {ex.Message}");
				return false;
			}
		}

		public List<VehicleModel> GetVehiclesByUserId(int userId)
		{
			List<VehicleModel> vehicles = new List<VehicleModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, producer_id, vehicle_type_id, wheel_drive_type_id, transmission_type_id, model, 
                    mileage, engine_displacement, horsepower, production_year, user_id FROM ""vehicle"" WHERE user_id = @UserId";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("UserId", userId);

						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var vehicle = new VehicleModel
								{
									Id = Convert.ToInt32(reader["id"]),
									ProducerId = Convert.ToInt32(reader["producer_id"]),
									VehicleTypeId = Convert.ToInt32(reader["vehicle_type_id"]),
									WheelDriveTypeId = Convert.ToInt32(reader["wheel_drive_type_id"]),
									TransmissionTypeId = Convert.ToInt32(reader["transmission_type_id"]),
									Model = reader["model"].ToString(),
									Mileage = Convert.ToDecimal(reader["mileage"]),
									EngineDisplacement = Convert.ToDecimal(reader["engine_displacement"]),
									Horsepower = Convert.ToInt32(reader["horsepower"]),
									ProductionYear = Convert.ToDateTime(reader["production_year"]),
									UserId = Convert.ToInt32(reader["user_id"])
								};

								vehicles.Add(vehicle);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список транспортных средств для пользователя с ID {userId}");
				return vehicles;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка транспортных средств для пользователя с ID {userId}: {ex.Message}");
				return null;
			}
		}


		public bool DeleteVehicle(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"DELETE FROM ""vehicle"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Удалено транспортное средство: ID {id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Транспортное средство с ID {id} не удалено");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при удалении транспортного средства: {ex.Message}");
				return false;
			}
		}
	}
}