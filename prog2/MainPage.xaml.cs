using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using prog2.Models;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;

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
        var viewModel = BindingContext as MainPageViewModel;
        if (viewModel != null)
        {
            var addresses = await _databaseService.GetAddressesAsync();
            viewModel.Addresses = new ObservableCollection<Address>(addresses);
        }
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

            // Ensure that the first row will be deleted if vorname==vorname
            if (addresses.Count > 0 && addresses[0].Vorname == "vorname")
            {
                addresses.RemoveAt(0); // delete first row
            }

            // Save locations and addresses
            foreach (var address in addresses)
            {
                if (address.Location != null)
                {
                    await _databaseService.SaveLocationAsync(address.Location);
                }
            }

            await _databaseService.SaveAddressesAsync(addresses);

            // Reload the addresses
            var viewModel = BindingContext as MainPageViewModel;
            if (viewModel != null)
            {
                var savedAddresses = await _databaseService.GetAddressesAsync();
                viewModel.Addresses = new ObservableCollection<Address>(savedAddresses);
            }
        }
        else
        {
            await DisplayAlert("Error", "No file selected.", "OK");
        }
    }

    private async void DeleteAllAddresses_Click(object sender, EventArgs e)
    {
        await _databaseService.DeleteAllAddressesAsync();

        // Clear UI
        var viewModel = BindingContext as MainPageViewModel;
        if (viewModel != null)
        {
            viewModel.Addresses.Clear();
        }
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as MainPageViewModel;
        if (viewModel != null)
        {
            foreach (var address in viewModel.ModifiedAddresses)
            {
                await _databaseService.SaveAddressAsync(address);
            }

            viewModel.IsSaveButtonVisible = false;

            var savedAddresses = await _databaseService.GetAddressesAsync();
            viewModel.Addresses = new ObservableCollection<Address>(savedAddresses);
        }
    }
}