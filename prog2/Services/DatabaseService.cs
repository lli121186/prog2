using prog2.Models;using SQLite;using System;using System.Collections.Generic;using System.IO;using System.Threading.Tasks;public class DatabaseService{    private readonly SQLiteAsyncConnection _database;    public DatabaseService()    {        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AddressesV2.db3");        _database = new SQLiteAsyncConnection(dbPath);        _database.CreateTableAsync<Address>().Wait();        _database.CreateTableAsync<Locations>().Wait();    }

    public async Task<List<Address>> GetAddressesAsync()
    {
        var addresses = await _database.Table<Address>().ToListAsync();

        foreach (var address in addresses)
        {
            address.Location = await _database.FindAsync<Locations>(address.LocationId);
        }

        return addresses;
    }    public async Task<int> SaveAddressAsync(Address address)    {        try        {            return await _database.InsertOrReplaceAsync(address);        }        catch (Exception ex)        {            Console.WriteLine($"Error saving address: {ex.Message}");            return -1; // Or any other error code or value
        }    }    public async Task<int> SaveAddressesAsync(List<Address> addresses)
    {
        try
        {
            foreach (var address in addresses)
            {
                var result = await SaveAddressAsync(address);
                if (result == -1) // Falls ein Fehler auftritt
                {
                    throw new Exception($"Fehler beim Speichern der Adresse mit ID: {address.Id}");
                }
            }
            return 1; // Erfolg
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving addresses: {ex.Message}");
            return -1; // Fehlercode
        }
    }
    public async Task<int> SaveLocationAsync(Locations location)    {        try        {            await _database.InsertOrReplaceAsync(location);            return location.Id; // Return the ID of the saved location
        }        catch (Exception ex)        {            Console.WriteLine($"Error saving location: {ex.Message}");            throw;        }    }    public async Task<int> DeleteAllAddressesAsync()    {        try        {            return await _database.DeleteAllAsync<Address>();        }        catch (Exception ex)        {            Console.WriteLine($"Error deleting all addresses: {ex.Message}");            return -1; // Or any other error code or value
        }    }    public async Task<int> DeleteAllLocationsAsync()    {        try        {            return await _database.DeleteAllAsync<Locations>();        }        catch (Exception ex)        {            Console.WriteLine($"Error deleting all locations: {ex.Message}");            return -1; // Or any other error code or value
        }    }}