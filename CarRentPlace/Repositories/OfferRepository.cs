using Npgsql;
using System;
using System.Collections.Generic;
using CarRentPlace.Models;

namespace CarRentPlace.Repositories
{
	public class OfferRepository
	{
		private string _connectionString;

		public OfferRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public OfferModel CreateOffer(int vehicleId, int userId, DateTime publicationDate, string description, decimal price, string location, bool show, bool approved)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"INSERT INTO ""offer"" (vehicle_id, user_id, publication_date, description, price, location, show, approved) 
                            VALUES (@VehicleId, @UserId, @PublicationDate, @Description, @Price, @Location, @Show, @Approved) 
                            RETURNING id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("VehicleId", vehicleId);
						command.Parameters.AddWithValue("UserId", userId);
						command.Parameters.AddWithValue("PublicationDate", publicationDate);
						command.Parameters.AddWithValue("Description", description);
						command.Parameters.AddWithValue("Price", price);
						command.Parameters.AddWithValue("Location", location);
						command.Parameters.AddWithValue("Show", show);
						command.Parameters.AddWithValue("Approved", approved);

						int id = (int)command.ExecuteScalar();

						var newOffer = new OfferModel
						{
							Id = id,
							VehicleId = vehicleId,
							UserId = userId,
							PublicationDate = publicationDate,
							Description = description,
							Price = price,
							Location = location,
							Show = show,
							Approved = approved
						};

						Console.WriteLine($"Log ----> Создано предложение: ID {id}");
						return newOffer;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при создании предложения: {ex.Message}");
				return null;
			}
		}

		public OfferModel GetOfferById(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT vehicle_id, user_id, publication_date, description, price, location, show, approved 
                            FROM ""offer"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								var offer = new OfferModel
								{
									Id = id,
									VehicleId = Convert.ToInt32(reader["vehicle_id"]),
									UserId = Convert.ToInt32(reader["user_id"]),
									PublicationDate = Convert.ToDateTime(reader["publication_date"]),
									Description = reader["description"].ToString(),
									Price = Convert.ToDecimal(reader["price"]),
									Location = reader["location"].ToString(),
									Show = Convert.ToBoolean(reader["show"]),
									Approved = Convert.ToBoolean(reader["approved"])
								};

								Console.WriteLine($"Log ----> Получено предложение: ID {id}");
								return offer;
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Предложение с ID {id} не найдено");
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении предложения с ID {id}: {ex.Message}");
				return null;
			}
		}

		public List<OfferModel> GetAllOffers()
		{
			List<OfferModel> offers = new List<OfferModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, vehicle_id, user_id, publication_date, description, price, location, show, approved FROM ""offer""";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var offer = new OfferModel
								{
									Id = Convert.ToInt32(reader["id"]),
									VehicleId = Convert.ToInt32(reader["vehicle_id"]),
									UserId = Convert.ToInt32(reader["user_id"]),
									PublicationDate = Convert.ToDateTime(reader["publication_date"]),
									Description = reader["description"].ToString(),
									Price = Convert.ToDecimal(reader["price"]),
									Location = reader["location"].ToString(),
									Show = Convert.ToBoolean(reader["show"]),
									Approved = Convert.ToBoolean(reader["approved"])
								};

								offers.Add(offer);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список всех предложений");
				return offers;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка предложений: {ex.Message}");
				return null;
			}
		}

		public bool UpdateOffer(OfferModel offer)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"UPDATE ""offer"" SET vehicle_id = @VehicleId, user_id = @UserId, publication_date = @PublicationDate, 
                            description = @Description, price = @Price, location = @Location, show = @Show, approved = @Approved 
                            WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", offer.Id);
						command.Parameters.AddWithValue("VehicleId", offer.VehicleId);
						command.Parameters.AddWithValue("UserId", offer.UserId);
						command.Parameters.AddWithValue("PublicationDate", offer.PublicationDate);
						command.Parameters.AddWithValue("Description", offer.Description);
						command.Parameters.AddWithValue("Price", offer.Price);
						command.Parameters.AddWithValue("Location", offer.Location);
						command.Parameters.AddWithValue("Show", offer.Show);
						command.Parameters.AddWithValue("Approved", offer.Approved);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Обновлено предложение: ID {offer.Id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Предложение с ID {offer.Id} не обновлено");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при обновлении предложения с ID {offer.Id}: {ex.Message}");
				return false;
			}
		}

		public bool DeleteOffer(int id)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"DELETE FROM ""offer"" WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Удалено предложение: ID {id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Предложение с ID {id} не удалено");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при удалении предложения с ID {id}: {ex.Message}");
				return false;
			}
		}

		public List<OfferModel> GetOffersByUserId(int userId)
		{
			List<OfferModel> offers = new List<OfferModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, vehicle_id, user_id, publication_date, description, price, location, show, approved 
                        FROM ""offer"" WHERE user_id = @UserId";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("UserId", userId);

						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var offer = new OfferModel
								{
									Id = Convert.ToInt32(reader["id"]),
									VehicleId = Convert.ToInt32(reader["vehicle_id"]),
									UserId = Convert.ToInt32(reader["user_id"]),
									PublicationDate = Convert.ToDateTime(reader["publication_date"]),
									Description = reader["description"].ToString(),
									Price = Convert.ToDecimal(reader["price"]),
									Location = reader["location"].ToString(),
									Show = Convert.ToBoolean(reader["show"]),
									Approved = Convert.ToBoolean(reader["approved"])
								};

								offers.Add(offer);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список предложений для пользователя: ID {userId}");
				return offers;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка предложений для пользователя с ID {userId}: {ex.Message}");
				return null;
			}
		}


		public bool UpdateOfferShowStatus(int id, bool showStatus)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"UPDATE ""offer"" SET show = @Show WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);
						command.Parameters.AddWithValue("Show", showStatus);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Изменен статус Show для предложения: ID {id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Не удалось изменить статус Show для предложения: ID {id}");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при изменении статуса Show для предложения с ID {id}: {ex.Message}");
				return false;
			}
		}

		public bool UpdateOfferApprovedStatus(int id, bool approvedStatus)
		{
			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"UPDATE ""offer"" SET approved = @Approved WHERE id = @Id";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("Id", id);
						command.Parameters.AddWithValue("Approved", approvedStatus);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine($"Log ----> Изменен статус Approved для предложения: ID {id}");
							return true;
						}
					}
				}

				Console.WriteLine($"Log ----> Не удалось изменить статус Approved для предложения: ID {id}");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при изменении статуса Approved для предложения с ID {id}: {ex.Message}");
				return false;
			}
		}

		public List<OfferModel> GetApprovedOffers()
		{
			List<OfferModel> approvedOffers = new List<OfferModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, vehicle_id, user_id, publication_date, description, price, location, show, approved 
                        FROM ""offer"" WHERE show = true AND approved = true";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var offer = new OfferModel
								{
									Id = Convert.ToInt32(reader["id"]),
									VehicleId = Convert.ToInt32(reader["vehicle_id"]),
									UserId = Convert.ToInt32(reader["user_id"]),
									PublicationDate = Convert.ToDateTime(reader["publication_date"]),
									Description = reader["description"].ToString(),
									Price = Convert.ToDecimal(reader["price"]),
									Location = reader["location"].ToString(),
									Show = Convert.ToBoolean(reader["show"]),
									Approved = Convert.ToBoolean(reader["approved"])
								};

								approvedOffers.Add(offer);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список одобренных предложений");
				return approvedOffers;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка одобренных предложений: {ex.Message}");
				return null;
			}
		}

		public List<OfferModel> GetUnapprovedOffers()
		{
			List<OfferModel> unapprovedOffers = new List<OfferModel>();

			try
			{
				using (var connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					var sql = @"SELECT id, vehicle_id, user_id, publication_date, description, price, location, show, approved 
                        FROM ""offer"" WHERE approved = false";

					using (var command = new NpgsqlCommand(sql, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var offer = new OfferModel
								{
									Id = Convert.ToInt32(reader["id"]),
									VehicleId = Convert.ToInt32(reader["vehicle_id"]),
									UserId = Convert.ToInt32(reader["user_id"]),
									PublicationDate = Convert.ToDateTime(reader["publication_date"]),
									Description = reader["description"].ToString(),
									Price = Convert.ToDecimal(reader["price"]),
									Location = reader["location"].ToString(),
									Show = Convert.ToBoolean(reader["show"]),
									Approved = Convert.ToBoolean(reader["approved"])
								};

								unapprovedOffers.Add(offer);
							}
						}
					}
				}

				Console.WriteLine($"Log ----> Получен список неподтвержденных предложений");
				return unapprovedOffers;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Log ----> Ошибка при получении списка неподтвержденных предложений: {ex.Message}");
				return null;
			}
		}
	}
}
