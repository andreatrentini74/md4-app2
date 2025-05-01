using Microsoft.Maui.Controls;

namespace QRBarcodeScannerApp
{
    public partial class App : Application
    {
        public static AppShell appShell;
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(appShell = new AppShell());
        }
        protected async override void OnStart()
        {
            base.OnStart();
            // Verifica autorizzazione fotocamera all'avvio
            await VerificaAutorizzazioneFotocameraAllAvvio();
        }
        private async Task VerificaAutorizzazioneFotocameraAllAvvio()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();

                    if (status != PermissionStatus.Granted)
                    {
                        // L'utente ha negato l'autorizzazione
                        await MainPage.DisplayAlert(
                            "Autorizzazione Fotocamera",
                            "L'app richiede l'autorizzazione alla fotocamera per funzionare correttamente. " +
                            "Puoi modificare questa impostazione nelle preferenze del dispositivo.",
                            "Ho capito");
                    }
                }

                // Salva lo stato dell'autorizzazione per riferimento futuro
                // Ad esempio in Preferences
                Microsoft.Maui.Storage.Preferences.Set("CameraPermissionStatus", status.ToString());
            }
            catch (Exception ex)
            {
                // Gestisci eventuali errori
                Console.WriteLine($"Errore nel controllo dell'autorizzazione fotocamera: {ex.Message}");
            }
        }
    }
}