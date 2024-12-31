using System.Collections.ObjectModel;
using System.ComponentModel;
using prog2.Models;

public class MainPageViewModel : INotifyPropertyChanged
{
    private bool _isSaveButtonVisible;
    private ObservableCollection<Address> _addresses;
    private List<Address> _modifiedAddresses = new List<Address>();

    public bool IsSaveButtonVisible
    {
        get => _isSaveButtonVisible;
        set
        {
            _isSaveButtonVisible = value;
            OnPropertyChanged(nameof(IsSaveButtonVisible));
        }
    }

    public ObservableCollection<Address> Addresses
    {
        get => _addresses;
        set
        {
            _addresses = value;
            OnPropertyChanged(nameof(Addresses));
        }
    }

    public List<Address> ModifiedAddresses
    {
        get => _modifiedAddresses;
        set
        {
            _modifiedAddresses = value;
            OnPropertyChanged(nameof(ModifiedAddresses));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
