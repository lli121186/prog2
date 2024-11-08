using prog2.Models;
using SQLite;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Addresses.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Address>().Wait();
        _database.CreateTableAsync<prog2.Models.Location>().Wait();
    }

    public Task<List<Address>> GetAddressesAsync()
    {
        return _database.Table<Address>().ToListAsync();
    }

    public Task<int> SaveAddressAsync(Address address)
    {
        return _database.InsertAsync(address);
    }

    public Task<int> SaveAddressesAsync(List<Address> addresses)
    {
        return _database.InsertAllAsync(addresses);
    }

    public Task<int> SaveLocationAsync(prog2.Models.Location location)
    {
        return _database.InsertAsync(location);
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
