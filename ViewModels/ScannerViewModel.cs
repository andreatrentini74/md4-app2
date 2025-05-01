using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QRBarcodeScannerApp.Models;
using QRBarcodeScannerApp.Pages;
using QRBarcodeScannerApp.Services;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace QRBarcodeScannerApp.ViewModels
{
    public class ScannerViewModel : INotifyPropertyChanged
    {
        private string _qrCode;
        private string _barcode;
        private string _statusMessage;
        private bool _isBusy;

        private readonly ScanService _scanService;
        private readonly AppSettings _settings;

        public ScannerViewModel(AppSettings settings)
        {
            _qrCode = "22/02738_A316-TU101X5,74_003177_Y";
            _barcode = "22/36983_3394224-0000|1_0";
            _settings = settings;
            _scanService = new ScanService(_settings);

            ScanQRCommand = new Command(async () => await ScanQRCodeAsync());
            ScanBarcodeCommand = new Command(async () => await ScanBarcodeAsync());
            SendDataCommand = new Command(async () => await SendDataAsync(), () => !string.IsNullOrEmpty(QRCode) && !string.IsNullOrEmpty(Barcode));
            NavigateToConfigCommand = new Command(async () =>
            {
                try
                {
                    await Shell.Current.GoToAsync(nameof(ConfigPage));
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Errore: {ex}";
                }
            });
        }

        public string QRCode
        {
            get => _qrCode;
            set
            {
                if (_qrCode != value)
                {
                    _qrCode = value;
                    OnPropertyChanged();
                    ((Command)SendDataCommand).ChangeCanExecute();
                }
            }
        }

        public string Barcode
        {
            get => _barcode;
            set
            {
                if (_barcode != value)
                {
                    _barcode = value;
                    OnPropertyChanged();
                    ((Command)SendDataCommand).ChangeCanExecute();
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

        public ICommand ScanQRCommand { get; }
        public ICommand ScanBarcodeCommand { get; }
        public ICommand SendDataCommand { get; }
        public ICommand NavigateToConfigCommand { get; }

        private async Task ScanQRCodeAsync()
        {
            try
            {
                IsBusy = true;

                // Utilizziamo il componente CameraBarcodeReaderView di ZXing.Net.Maui
                // che è compatibile con AndroidX
                var result = await OpenBarcodeReaderPage(BarcodeFormat.QrCode);

                if (!string.IsNullOrEmpty(result))
                {
                    QRCode = result;
                    StatusMessage = "QR Code scansionato con successo!";
                }
                else
                {
                    StatusMessage = "Scansione QR Code annullata";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Errore durante la scansione del QR Code: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ScanBarcodeAsync()
        {
            try
            {
                IsBusy = true;

                // Filtriamo solo per i formati barcode standard
                var formats = BarcodeFormat.Code128 | BarcodeFormat.Code39 | BarcodeFormat.Ean13 | BarcodeFormat.Ean8;
                var result = await OpenBarcodeReaderPage(formats);

                if (!string.IsNullOrEmpty(result))
                {
                    Barcode = result;
                    StatusMessage = "Barcode scansionato con successo!";
                }
                else
                {
                    StatusMessage = "Scansione Barcode annullata";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Errore durante la scansione del Barcode: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<string> OpenBarcodeReaderPage(BarcodeFormat formats)
        {
            // Creiamo una TaskCompletionSource per aspettare il risultato
            var tcs = new TaskCompletionSource<string>();

            // Creiamo una nuova pagina con il camera view reader
            var scanPage = new ContentPage
            {
                Title = "Scansione"
            };

            var barcodeReader = new CameraBarcodeReaderView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Options = new BarcodeReaderOptions { Formats = formats, AutoRotate = true, Multiple = false, TryHarder = true },
                IsDetecting = true
            };

            // Aggiungiamo un pulsante di cancellazione
            var cancelButton = new Button
            {
                Text = "Annulla",
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 30)
            };

            cancelButton.Clicked += async (s, e) =>
            {
                await Shell.Current.Navigation.PopModalAsync();
                tcs.SetResult(string.Empty);
            };

            // Handler per il rilevamento del barcode
            barcodeReader.BarcodesDetected += async (s, e) =>
            {
                if (e.Results.FirstOrDefault() is BarcodeResult result)
                {
                    // Disabilitiamo il rilevamento per evitare rilevamenti multipli
                    barcodeReader.IsDetecting = false;

                    // Otteniamo il valore del codice
                    string value = result.Value;

                    // Torniamo alla pagina precedente
                    await Shell.Current.Navigation.PopModalAsync();

                    // Impostiamo il risultato
                    tcs.SetResult(value);
                }
            };

            // Creiamo un contenitore per i controlli
            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) }
                }
            };

            grid.Add(barcodeReader, 0, 0);
            grid.Add(cancelButton, 0, 1);

            scanPage.Content = grid;

            // Mostriamo la pagina
            await Shell.Current.Navigation.PushModalAsync(scanPage);

            // Attendiamo il risultato
            return await tcs.Task;
        }

        private async Task SendDataAsync()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Invio dati in corso...";

                var scanResult = new ScanResult
                {
                    QrCode = QRCode,
                    BarCode = Barcode
                };

                var success = await _scanService.SendScanResultAsync(scanResult.ConcatenatedResult(_settings));

                if (success)
                {
                    StatusMessage = "Dati inviati con successo!";
                    //QRCode = string.Empty;
                    //Barcode = string.Empty;
                }
                else
                {
                    StatusMessage = "Errore durante l'invio dei dati. Controlla la configurazione.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Errore: {ex.Message}";
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