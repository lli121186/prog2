using CsvHelper;
using System.Globalization;
using CommunityToolkit.Maui;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;

namespace prog2
{
    // Define a class that represents the structure of your CSV records
    public class CsvRecord
    {
        public string Column1 { get; set; } // Adjust properties based on your CSV structure
        public string Column2 { get; set; }
        // Add more properties as needed
    }

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnOpenCsvClicked(object sender, EventArgs e)
        {
            try
            {
                var csvFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "text/csv" } },
                    { DevicePlatform.iOS, new[] { "public.comma-separated-values-text" } },
                    { DevicePlatform.WinUI, new[] { ".csv" } }
                });

                var fileResult = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Bitte wähle eine CSV-Datei aus",
                    FileTypes = csvFileType
                });

                // Check if the user cancelled the file picker
                if (fileResult == null)
                {
                    return; // Exit if no file was selected
                }

                var stream = await fileResult.OpenReadAsync();
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<CsvRecord>().ToList(); // Use strongly typed class
                    csvCollectionView.ItemsSource = records; // Set the items source for CollectionView
                }
            }
            catch (CsvHelperException csvEx)
            {
                await DisplayAlert("Fehler", "Fehler beim Verarbeiten der CSV-Datei: " + csvEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fehler", "Fehler beim Öffnen der Datei: " + ex.Message, "OK");
            }
        }
    }
}
