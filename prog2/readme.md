# Projekt: Address Management System

## Beschreibung
Das Address Management System ist eine Anwendung zur Verwaltung von Adressdaten mit Unterst�tzung f�r CSV-Import, Datenbankintegration und asynchronen Operationen. Es wurde entwickelt, um eine benutzerfreundliche Oberfl�che und saubere Code-Architektur zu bieten.

## Features
- **CSV-Import und -Export**: Adressdaten k�nnen �ber CSV-Dateien importiert und exportiert werden.
- **SQLite-Datenbankintegration**: Speicherung und Verwaltung von Daten mithilfe von SQLite.
- **Fremdschl�sselbeziehungen**: Verkn�pfung von Adressen mit zugeh�rigen Standorten.
- **Reaktive Benutzeroberfl�che**: Nutzung von `CollectionView` und anderen Steuerelementen f�r eine intuitive Bedienung.
- **Non-blocking Calls**: Asynchrone Methoden sorgen f�r eine reaktionsf�hige Benutzeroberfl�che.
- **Modularer Code**: Saubere Trennung von Logik in Serviceklassen und UI-Komponenten.

## Installation
1. **Voraussetzungen:**
   - .NET MAUI SDK
   - Visual Studio 2022 (oder h�her) mit MAUI-Workload
2. **Repository klonen:**
   ```bash
   git clone https://github.com/dein-repo/address-management-system.git
   cd address-management-system
   ```
3. **Abh�ngigkeiten wiederherstellen:**
   ```bash
   dotnet restore
   ```
4. **Projekt starten:**
   ```bash
   dotnet build
   dotnet run
   ```

## Verwendung
1. **CSV-Datei laden:**
   - Klicken Sie auf die Schaltfl�che "Load CSV", um eine Datei auszuw�hlen und die Adressen zu importieren.
2. **Daten anzeigen:**
   - Die importierten Adressen werden in der �bersichtsliste angezeigt.
3. **Datenbankaktionen:**
   - Speichern, L�schen oder Bearbeiten von Adressen in der SQLite-Datenbank.
4. **Alle Adressen l�schen:**
   - Klicken Sie auf die Schaltfl�che "Delete All", um die Datenbank zu leeren.

## Code-Architektur
- **DatabaseService**: Serviceklasse f�r die Interaktion mit der SQLite-Datenbank.
- **MainPage**: Hauptbenutzeroberfl�che mit Funktionen wie CSV-Import und Datenanzeige.
- **CsvHandler**: Hilfsklasse f�r das Parsen und Laden von CSV-Dateien.
- **ViewModel**: Verwaltung des Zustands der Benutzeroberfl�che.

## Beispiel-Code
### CSV-Datei laden:
```csharp
private async void LoadCsv_Click(object sender, EventArgs e)
{
    var result = await FilePicker.PickAsync();
    if (result != null)
    {
        var filePath = result.FullPath;
        List<Address> addresses = await Task.Run(() => _csvHandler.LoadCsv(filePath));
        await _databaseService.SaveAddressesAsync(addresses);
        AddressCollectionView.ItemsSource = await _databaseService.GetAddressesAsync();
    }
}
```

### SQLite-Datenbankzugriff:
```csharp
public async Task<List<Address>> GetAddressesAsync()
{
    var addresses = await _database.Table<Address>().ToListAsync();
    foreach (var address in addresses)
    {
        if (address.LocationId != 0)
        {
            address.Location = await _database.FindAsync<Location>(address.LocationId);
        }
    }
    return addresses;
}
```

## Geplante Erweiterungen
- **Datenbankverschl�sselung**: Implementierung von SQLCipher f�r erh�hte Sicherheit.
- **Mandantenf�higkeit**: Unterst�tzung f�r mehrere Datenbanken pro Benutzer.
- **Unit Tests**: Einf�hrung von automatisierten Tests mit `xUnit`.

## Lizenz
Dieses Projekt steht unter der MIT-Lizenz. Weitere Informationen finden Sie in der Datei `LICENSE`.

## Autoren
- [Ihr Name/Teamname] - Entwicklung und Dokumentation

