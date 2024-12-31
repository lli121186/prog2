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
        BindingContext = new MainPageViewModel();
        InitializeAddressCollectionView();
    }

    private async void InitializeAddressCollectionView()
    {
        AddressCollectionView.ItemsSource = await _databaseService.GetAddressesAsync();
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
            
            //Ensure that the first row will be deleted if vorname==vorname
            if (addresses.Count > 0)
            {
                if (addresses[0].Vorname == "vorname")
                {
                    addresses.RemoveAt(0); // delete first row
                }
            }

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
            AddressCollectionView.ItemsSource = null;
            AddressCollectionView.ItemsSource = savedAddresses;
        }
        else
        {
            await DisplayAlert("Error", "No file selected.", "OK");
        }
    }

    private async void DeleteAllAddresses_Click(object sender, EventArgs e)
    {
        await _databaseService.DeleteAllAddressesAsync();
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Error", "No file selected.", "OK");

        var viewModel = BindingContext as MainPageViewModel;
        if (viewModel != null)
        {
            // Save each modified address to the database
            foreach (var address in viewModel.ModifiedAddresses)
            {
                await _databaseService.SaveAddressAsync(address);
            }

            // After saving, hide the save button
            viewModel.IsSaveButtonVisible = false;

            // Optionally, you can reload the addresses from the database to update the UI
            var savedAddresses = await _databaseService.GetAddressesAsync();
            AddressCollectionView.ItemsSource = null;
            AddressCollectionView.ItemsSource = savedAddresses;
        }
    }
}