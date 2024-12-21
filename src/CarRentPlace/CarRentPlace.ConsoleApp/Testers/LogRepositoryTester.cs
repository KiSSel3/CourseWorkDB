using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.ConsoleApp.Testers;

//TODO: переделать (user должен существовать)
public class LogRepositoryTester
{
    private readonly ILogRepository _repository;

    public LogRepositoryTester(ILogRepository repository)
    {
        _repository = repository;
    }

    public async Task RunTestsAsync()
    {
        Console.WriteLine("\n==================== TESTING LogRepository ====================\n");

        // Test AddAsync
        Console.WriteLine("Adding a new log entry...");
        var newLog = new Log
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ActionType = "TestAction",
            EntityType = "TestEntity",
            EntityId = Guid.NewGuid(),
            OldValues = "OldValue",
            NewValues = "NewValue",
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
        await _repository.AddAsync(newLog);
        Console.WriteLine("Log entry added successfully.\n");

        // Test GetByIdAsync
        Console.WriteLine("Fetching log entry by Id...");
        var fetchedLog = await _repository.GetByIdAsync(newLog.Id, includeDeleted: false);
        if (fetchedLog != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(fetchedLog);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Log entry not found by Id.\n");
        }

        // Test GetAllAsync
        Console.WriteLine("Fetching all log entries...");
        var allLogs = await _repository.GetAllAsync(includeDeleted: false);
        foreach (var log in allLogs)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(log);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test UpdateAsync
        Console.WriteLine("Updating log entry...");
        if (fetchedLog != null)
        {
            fetchedLog.ActionType = "UpdatedAction";
            fetchedLog.NewValues = "UpdatedValue";
            await _repository.UpdateAsync(fetchedLog);
            Console.WriteLine("Log entry updated successfully.\n");
        }

        // Test GetByUserIdAsync
        Console.WriteLine("Fetching log entries by UserId...");
        var logsByUser = await _repository.GetByUserIdAsync(newLog.UserId.Value, includeDeleted: false);
        foreach (var log in logsByUser)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(log);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test GetByActionTypeAsync
        Console.WriteLine("Fetching log entries by ActionType...");
        var logsByAction = await _repository.GetByActionTypeAsync("UpdatedAction", includeDeleted: false);
        foreach (var log in logsByAction)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(log);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test SoftDeleteAsync
        Console.WriteLine("Soft deleting log entry...");
        if (fetchedLog != null)
        {
            await _repository.SoftDeleteAsync(fetchedLog.Id);
            Console.WriteLine("Log entry soft deleted successfully.\n");
        }

        // Test GetByIdAsync with includeDeleted = true
        Console.WriteLine("Fetching log entry by Id (include deleted)...");
        if (fetchedLog != null)
        {
            var logById = await _repository.GetByIdAsync(fetchedLog.Id, includeDeleted: true);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(logById);
            Console.ResetColor();
        }
        Console.WriteLine();

        // Test DeleteAsync
        Console.WriteLine("Deleting log entry...");
        if (fetchedLog != null)
        {
            await _repository.DeleteAsync(fetchedLog.Id);
            Console.WriteLine("Log entry deleted successfully.\n");
        }

        // Final output of all logs
        Console.WriteLine("Fetching all log entries after tests...");
        var finalLogs = await _repository.GetAllAsync(includeDeleted: true);
        foreach (var log in finalLogs)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(log);
            Console.ResetColor();
        }

        Console.WriteLine("\n==================== TESTING COMPLETE ====================\n");
    }
}