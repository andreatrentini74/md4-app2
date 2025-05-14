using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QRBarcodeScannerApp.Services;

namespace QRBarcodeScannerApp.ViewModels
{
    public class ConfigViewModel : INotifyPropertyChanged
    {
        private AppSettings _settings;
        private string _apiEndpoint;
        private int _port;
        private string _statusMessage;
        private bool _isBusy;
        private int _ratio;
        private int _positionX;
        private int _positionY;
        private int _positionYRiga1;
        private int _positionYRiga2;

        public ConfigViewModel(AppSettings settings)
        {
            _settings = settings;

            IPAddress = _settings.IPAddress;
            Port = _settings.Port;
            Ratio = _settings.Ratio;
            PositionX = _settings.PositionX;
            PositionY = _settings.PositionY;
            PositionYRiga1 = _settings.PositionYRiga1;
            PositionYRiga2 = _settings.PositionYRiga2;

            SaveCommand = new Command(SaveSettings);
            BackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        public string IPAddress
        {
            get => _apiEndpoint;
            set
            {
                if (_apiEndpoint != value)
                {
                    _apiEndpoint = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                if (_port != value)
                {
                    _port = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Ratio
        {
            get => _ratio;
            set
            {
                if (_ratio != value)
                {
                    _ratio = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PositionX
        {
            get => _positionX;
            set
            {
                if (_positionX != value)
                {
                    _positionX = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PositionY
        {
            get => _positionY;
            set
            {
                if (_positionY != value)
                {
                    _positionY = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PositionYRiga1
        {
            get => _positionYRiga1;
            set
            {
                if (_positionYRiga1 != value)
                {
                    _positionYRiga1 = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PositionYRiga2
        {
            get => _positionYRiga2;
            set
            {
                if (_positionYRiga2 != value)
                {
                    _positionYRiga2 = value;
                    OnPropertyChanged();
                }
            }
        }


        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand BackCommand { get; }

        private void SaveSettings()
        {
            IsBusy = true;

            try
            {
                _settings.IPAddress = IPAddress;
                _settings.Port = Port;
                _settings.Ratio = Ratio;
                _settings.PositionX = PositionX;
                _settings.PositionY = PositionY;
                _settings.PositionYRiga1 = PositionYRiga1;
                _settings.PositionYRiga2 = PositionYRiga2;
                _settings.Save();
                StatusMessage = "Impostazioni salvate con successo!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Errore nel salvataggio delle impostazioni: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}