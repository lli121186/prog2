using prog2.Models;
using SQLite;
using System.Net;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AddressesV2.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Address>().Wait();
        _database.CreateTableAsync<Locations>().Wait();
    }

    public async Task<List<Address>> GetAddressesAsync()
    {
        var addresses = await _database.Table<Address>().ToListAsync();

        // Populate Location for each Address
        foreach (var address in addresses)
        {
            if (address.LocationId != 0)
            {
                address.Location = await _database.FindAsync<Locations>(address.LocationId);
            }
        }

        return addresses;
    }


    public async Task<int> SaveAddressAsync(Address address)
    {
        try
        {
            return await _database.InsertOrReplaceAsync(address);
        }
        catch (Exception ex)
        {
            // Handle the exception here
            Console.WriteLine($"Error saving address: {ex.Message}");
            return -1; // Or any other error code or value
        }
    }

    public Task<int> SaveAddressesAsync(List<Address> addresses)
    {
        try
        {
            return _database.InsertAllAsync(addresses);
        }
        catch (Exception ex)
        {
            // Handle the exception here
            Console.WriteLine($"Error saving addresses: {ex.Message}");
            return Task.FromResult(-1); // Or any other error code or value
        }
    }

    public async Task<int> SaveLocationAsync(Locations location)
    {
        try
        {
            Console.WriteLine($"Inserting Location: Postleitzahl={location.Postleitzahl}, Ortschaft={location.Ortschaft}");
            var result = await _database.InsertAsync(location);
            Console.WriteLine($"Location saved with ID: {location.Id}");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving location: {ex.Message}");
            throw; // Optional: Ausnahme erneut werfen, um den Fehler zu verfolgen
        }
    }


    public Task<int> DeleteAllAddressesAsync()
    {
        try
        {
            return _database.DeleteAllAsync<Address>();
        }
        catch (Exception ex)
        {
            // Handle the exception here
            Console.WriteLine($"Error deleting all addresses: {ex.Message}");
            return Task.FromResult(-1); // Or any other error code or value
        }
    }

    public Task<int> DeleteAllLocationsAsync()
    {
        try
        {
            return _database.DeleteAllAsync<Locations>();
        }
        catch (Exception ex)
        {
            // Handle the exception here
            Console.WriteLine($"Error deleting all locations: {ex.Message}");
            return Task.FromResult(-1); // Or any other error code or value
        }
    }
}
