using prog2.Models;
using System.IO;

public class CsvHandler
{
    public List<Address> LoadCsv(string filePath)
    {
        var addresses = new List<Address>();

        // Read CSV file line by line
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var columns = line.Split(';'); // Split by semicolon instead of comma

            // Create Address object
            var address = new Address
            {
                Vorname = columns[0],
                Name = columns[1],
                Firma = columns[2],
                Strasse = columns[3],
                Hausnummer = columns[4],
                Location = new prog2.Models.Location
                {
                    Postleitzahl = columns[5],
                    Ortschaft = columns[6]
                }
            };

            addresses.Add(address);
        }

        return addresses;
    }
}
