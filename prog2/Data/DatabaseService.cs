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
        _database.CreateTableAsync<prog2.Models.Location>().Wait();
    }

    public async Task<List<Address>> GetAddressesAsync()
    {
        var addresses = await _database.Table<Address>().ToListAsync();

        // Populate Location for each Address
        foreach (var address in addresses)
        {
            if (address.LocationId != 0)
            {
                address.Location = await _database.FindAsync<prog2.Models.Location>(address.LocationId);
            }
        }

        return addresses;
    }


    public Task<int> SaveAddressAsync(Address address)
    {
        return _database.InsertOrReplaceAsync(address);
    }

    public Task<int> SaveAddressesAsync(List<Address> addresses)
    {
        return _database.InsertAllAsync(addresses);
    }

    public Task<int> SaveLocationAsync(prog2.Models.Location location)
    {
        return _database.InsertOrReplaceAsync(location);
    }

    public Task<int> DeleteAllAddressesAsync()
    {
        return _database.DeleteAllAsync<Address>();
    }

    public Task<int> DeleteAllLocationsAsync()
    {
        return _database.DeleteAllAsync<prog2.Models.Location>();
    }
}
