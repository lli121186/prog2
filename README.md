# Projekt: Address Management System

## Beschreibung
Das Address Management System ist eine Anwendung zur Verwaltung von Adressdaten mit Unterstützung für CSV-Import, Datenbankintegration und asynchronen Operationen. Es wurde entwickelt, um eine benutzerfreundliche Oberfläche und saubere Code-Architektur zu bieten.

## Features
- **CSV-Import und -Export**: Adressdaten können über CSV-Dateien importiert und exportiert werden.
- **SQLite-Datenbankintegration**: Speicherung und Verwaltung von Daten mithilfe von SQLite.
- **Fremdschlüsselbeziehungen**: Verknüpfung von Adressen mit zugehörigen Standorten.
- **Reaktive Benutzeroberfläche**: Nutzung von `CollectionView` und anderen Steuerelementen für eine intuitive Bedienung.
- **Non-blocking Calls**: Asynchrone Methoden sorgen für eine reaktionsfähige Benutzeroberfläche.
- **Modularer Code**: Saubere Trennung von Logik in Serviceklassen und UI-Komponenten.

## Installation
1. **Voraussetzungen:**
   - .NET MAUI SDK
   - Visual Studio 2022 (oder höher) mit MAUI-Workload
2. **Repository klonen:**
   ```bash
   git clone https://github.com/dein-repo/address-management-system.git
   cd address-management-system
   ```
3. **Abhängigkeiten wiederherstellen:**
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
   - Klicken Sie auf die Schaltfläche "Load CSV", um eine Datei auszuwählen und die Adressen zu importieren.
2. **Daten anzeigen:**
   - Die importierten Adressen werden in der Übersichtsliste angezeigt.
3. **Datenbankaktionen:**
   - Speichern, Löschen oder Bearbeiten von Adressen in der SQLite-Datenbank.
4. **Alle Adressen löschen:**
   - Klicken Sie auf die Schaltfläche "Delete All", um die Datenbank zu leeren.

## Code-Architektur
- **DatabaseService**: Serviceklasse für die Interaktion mit der SQLite-Datenbank.
- **MainPage**: Hauptbenutzeroberfläche mit Funktionen wie CSV-Import und Datenanzeige.
- **CsvHandler**: Hilfsklasse für das Parsen und Laden von CSV-Dateien.
- **ViewModel**: Verwaltung des Zustands der Benutzeroberfläche.

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
- **Datenbankverschlüsselung**: Implementierung von SQLCipher für erhöhte Sicherheit.
- **Mandantenfähigkeit**: Unterstützung für mehrere Datenbanken pro Benutzer.
- **Unit Tests**: Einführung von automatisierten Tests mit `xUnit`.

## Lizenz
Dieses Projekt steht unter der MIT-Lizenz. Weitere Informationen finden Sie in der Datei `LICENSE`.

## Autoren
- [Ihr Name/Teamname] - Entwicklung und Dokumentation

