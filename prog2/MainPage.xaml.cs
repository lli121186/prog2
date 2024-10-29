using System.Formats.Asn1;
using System.Globalization;

namespace prog2
{
    public partial class MainPage : ContentPage
    {
            public MainPage() { InitializeComponent(); }
            private async void OnOpenCsvClicked(object sender, EventArgs e)
            {
                try
                {
                    var fileResult = await FilePicker.Default.PickAsync(new PickOptions
                    {
                        PickerTitle = "Bitte wähle eine CSV-Datei aus",
                        FileTypes = FilePickerFileType.Text
                    });
                    if (fileResult != null)
                    {
                        var stream = await fileResult.OpenReadAsync();
                        using (var reader = new StreamReader(stream))
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            var records = csv.GetRecords<dynamic>().ToList();
                            csvDataGrid.ItemsSource = records;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Fehler", "Fehler beim Öffnen der Datei: " + ex.Message, "OK");
                }
            }
        }
    }
 