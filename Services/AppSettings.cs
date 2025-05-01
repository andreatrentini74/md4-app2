using System.Text.Json;

namespace QRBarcodeScannerApp.Services
{
    public class AppSettings
    {
        public string IPAddress { get; set; }
        public ushort Port { get; set; }
        public int Ratio { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public void Save()
        {
            Preferences.Default.Set(nameof(IPAddress), IPAddress);
            Preferences.Default.Set(nameof(Port), Port);
            Preferences.Default.Set(nameof(Ratio), Ratio);
            Preferences.Default.Set(nameof(PositionX), PositionX);
            Preferences.Default.Set(nameof(PositionY), PositionY);
        }

        public static AppSettings Load()
        {
            return new AppSettings
            {
                IPAddress = Preferences.Default.Get(nameof(IPAddress), "192.168.1.200"),
                Port = (ushort)Preferences.Default.Get(nameof(Port), 9100),
                Ratio = Preferences.Default.Get(nameof(Ratio), 6),
                PositionX = Preferences.Default.Get(nameof(PositionX), 181),
                PositionY = Preferences.Default.Get(nameof(PositionY), 20)
            };
        }
    }

}