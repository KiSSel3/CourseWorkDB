using CarRentPlace.ConsoleApp.Testers;
using CarRentPlace.DAL.Repositories.Implementations;
using Npgsql;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string connectionString = "Host=localhost;Port=5432;Database=CarRentPlace;Username=postgres;Password=eshorka;";
        
        using var connection = new NpgsqlConnection(connectionString);

        //await RunCarBodyTypeTests(connection);
        
        //await RunCarBrandTests(connection);
        
        //await RunCarModelTests(connection);
        
        //await RunDriveTypeTests(connection);
        
        //await RunFeatureTests(connection);
        
        //await RunTransmissionTypeTests(connection);
        
        //await RunLogTests(connection);
        
        await RunCarTestsWithTransaction(connection);
    }

    private static async Task RunCarBodyTypeTests(NpgsqlConnection connection)
    {
        var repository = new CarBodyTypeRepository(connection);
        var tester = new CarBodyTypeRepositoryTester(repository);
        await tester.RunTestsAsync();
    }

    private static async Task RunCarBrandTests(NpgsqlConnection connection)
    {
        var repository = new CarBrandRepository(connection);
        var tester = new CarBrandRepositoryTester(repository);
        await tester.RunTestsAsync();
    }
    
    private static async Task RunCarModelTests(NpgsqlConnection connection)
    {
        var repository = new CarModelRepository(connection);
        var tester = new CarModelRepositoryTester(repository);
        await tester.RunTestsAsync();
    }
    
    private static async Task RunDriveTypeTests(NpgsqlConnection connection)
    {
        var repository = new DriveTypeRepository(connection);
        var tester = new DriveTypeRepositoryTester(repository);
        await tester.RunTestsAsync();
    }
    
    private static async Task RunFeatureTests(NpgsqlConnection connection)
    {
        var repository = new FeatureRepository(connection);
        var tester = new FeatureRepositoryTester(repository);
        await tester.RunTestsAsync();
    }
    
    private static async Task RunTransmissionTypeTests(NpgsqlConnection connection)
    {
        var repository = new TransmissionTypeRepository(connection);
        var tester = new TransmissionTypeRepositoryTester(repository);
        await tester.RunTestsAsync();
    }
    
    private static async Task RunLogTests(NpgsqlConnection connection)
    {
        var repository = new LogRepository(connection);
        var tester = new LogRepositoryTester(repository);
        await tester.RunTestsAsync();
    }
    
    private static async Task RunCarTestsWithTransaction(NpgsqlConnection connection)
    {
        await connection.OpenAsync();
    
        var carRepository = new CarRepository(connection);
        var carBrandRepository = new CarBrandRepository(connection);
        var carModelRepository = new CarModelRepository(connection);
        var carBodyTypeRepository = new CarBodyTypeRepository(connection);
        var transmissionTypeRepository = new TransmissionTypeRepository(connection);
        var driveTypeRepository = new DriveTypeRepository(connection);

        var tester = new CarRepositoryTester(
            carRepository,
            carBrandRepository,
            carModelRepository,
            carBodyTypeRepository,
            transmissionTypeRepository,
            driveTypeRepository);

        await tester.RunTestsAsync(connection);
    }
}
