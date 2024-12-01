using CarRentPlace.Client;

internal class Program
{
	private static string connString = "Host=localhost;Port=5432;Database=DealsPlaceDB;Username=postgres;Password=kissel";

	private static void Main(string[] args)
	{
		var client = new Client(connString);

		client.StartClient();
	}
}