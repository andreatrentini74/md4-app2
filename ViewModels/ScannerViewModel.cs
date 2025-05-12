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
        private string _lastPrint;
        private string _statusMessage;
        private bool _isBusy;

        private readonly ScanService _scanService;
        private readonly AppSettings _settings;

        public ScannerViewModel(AppSettings settings)
        {
#if DEBUG
            _qrCode = "22/02738_A316-TU101X5,74_003177_Y";
            _barcode = "22/36983_3394224-0000";
#endif
            _settings = settings;
            _scanService = new ScanService(_settings);

            ScanQRCommand = new Command(async () => await ScanQRCodeAsync());
            ScanBarcodeCommand = new Command(async () => await ScanBarcodeAsync());
            SendDataCommand = new Command(async () => await SendDataAsync(), () => !string.IsNullOrEmpty(QRCode) && !string.IsNullOrEmpty(Barcode));
            NavigateToConfigCommand = new Command(async () =>
            {
                try
                {
                    if (await App.appShell.DisplayPromptAsync("Password", "Inserire la password per modificare le impostazioni:", "Ok", "Annulla", keyboard: Keyboard.Plain) == "MC360MD4")
                        await Shell.Current.GoToAsync(nameof(ConfigPage));
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Errore: {ex}";
                }
            });
        }

        public string Scansiona_QR_Code { get => "Scansiona QR Code"; }
        public string Scansiona_Barcode { get => "Scansiona Barcode"; }
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

        public string LastPrint
        {
            get => _lastPrint;
            set
            {
                if (_lastPrint != value)
                {
                    _lastPrint = value;
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
                var result = await OpenBarcodeReaderPage(BarcodeFormat.QrCode, Scansiona_QR_Code);

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
                var result = await OpenBarcodeReaderPage(formats, Scansiona_Barcode);

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

        private async Task<string> OpenBarcodeReaderPage(BarcodeFormat formats, string title)
        {
            // Creiamo una TaskCompletionSource per aspettare il risultato
            var tcs = new TaskCompletionSource<string>();

            // Creiamo una nuova pagina con il camera view reader
            var scanPage = new ContentPage
            {
                Title = title
            };

            // Creiamo un contenitore per i controlli
            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) }
                }
            };

            var barcodeReader = new CameraBarcodeReaderView
            {
                //VerticalOptions = LayoutOptions.FillAndExpand,
                //HorizontalOptions = LayoutOptions.FillAndExpand,
                Options = new BarcodeReaderOptions { Formats = formats, AutoRotate = true, Multiple = false, TryHarder = true },
                IsDetecting = true,
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
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    barcodeReader.IsDetecting = false;
                    barcodeReader.IsEnabled = false;
                    grid.Remove(barcodeReader);
                });

                await Shell.Current.Navigation.PopModalAsync();
                tcs.SetResult(string.Empty);
            };

            // Handler per il rilevamento del barcode
            barcodeReader.BarcodesDetected += async (s, e) =>
            {
                if (e.Results.FirstOrDefault() is BarcodeResult result)
                {
                    // Disabilitiamo il rilevamento per evitare rilevamenti multipli

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        barcodeReader.IsDetecting = false;
                        barcodeReader.IsEnabled = false;
                    });

                    // Otteniamo il valore del codice
                    string value = result.Value;

                    // Torniamo alla pagina precedente
                    await Shell.Current.Navigation.PopModalAsync();

                    // Impostiamo il risultato
                    tcs.SetResult(value);
                }
            };

            grid.Add(new Views.CustomToolbar { Title = title }, 0, 0);
            grid.Add(barcodeReader, 0, 1);
            grid.Add(cancelButton, 0, 2);

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
                if (!await App.appShell.DisplayAlert("Stampa", "Vuoi stampare i codici letti?", "Conferma", "Annulla"))
                    return;
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
                    LastPrint = DateTime.Now.ToString("g");
                    StatusMessage = "Dati inviati con successo!";
                    if (await App.appShell.DisplayAlert("Azzeramento", "Vuoi azzerare i codici letti?", "Conferma", "Annulla"))
                    {
                        QRCode = string.Empty;
                        Barcode = string.Empty;
                    }
                }
                else
                {
                    StatusMessage = "Errore durante l'invio dei dati. Controlla la configurazione.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Errore durante l'invio dei dati: [{ex.GetType().Name}] - {ex.Message}";
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