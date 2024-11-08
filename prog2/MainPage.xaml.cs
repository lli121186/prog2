using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using prog2.Models;

namespace prog2;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private readonly CsvHandler _csvHandler;

    public MainPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        _csvHandler = new CsvHandler();
    }

    private async void LoadCsv_Click(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, new[] { ".csv" } },
            { DevicePlatform.iOS, new[] { "public.comma-separated-values-text" } },
            { DevicePlatform.Android, new[] { "text/csv" } }
        }),
            PickerTitle = "Please select a CSV file"
        });

        if (result != null)
        {
            var filePath = result.FullPath;
            List<Address> addresses = await Task.Run(() => _csvHandler.LoadCsv(filePath));

            // Ensure addresses are split correctly and save to the database
            foreach (var address in addresses)
            {
                // Handle Location creation separately and assign LocationId
                if (address.Location != null)
                {
                    // Save Location first and get the LocationId
                    await _databaseService.SaveLocationAsync(address.Location);
                }
            }

            // Save addresses to the database (including LocationId if necessary)
            await _databaseService.SaveAddressesAsync(addresses);

            // Reload the addresses from the database to update the UI
            var savedAddresses = await _databaseService.GetAddressesAsync();
            AddressCollectionView.ItemsSource = savedAddresses;
        }
        else
        {
            await DisplayAlert("Error", "No file selected.", "OK");
        }
    }

}
