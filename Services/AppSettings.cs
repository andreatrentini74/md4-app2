using System.Text.Json;

namespace md4.Services
{
    public class AppSettings
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public int Ratio { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int CaratteriPerRiga { get; set; }
        public int PositionYRiga1 { get; set; }
        public int PositionYRiga2 { get; set; }
        public int PositionXTesto { get; set; }

        public void Save()
        {
            Preferences.Default.Set(nameof(IPAddress), IPAddress);
            Preferences.Default.Set(nameof(Port), Port);
            Preferences.Default.Set(nameof(Ratio), Ratio);
            Preferences.Default.Set(nameof(PositionX), PositionX);
            Preferences.Default.Set(nameof(PositionY), PositionY);
            Preferences.Default.Set(nameof(CaratteriPerRiga), CaratteriPerRiga);
            Preferences.Default.Set(nameof(PositionYRiga1), PositionYRiga1);
            Preferences.Default.Set(nameof(PositionYRiga2), PositionYRiga2);
            Preferences.Default.Set(nameof(PositionXTesto), PositionXTesto);
        }

        public static AppSettings Load()
        {
            return new AppSettings
            {
                IPAddress = Preferences.Default.Get(nameof(IPAddress), "192.168.1.200"),
                Port = (ushort)Preferences.Default.Get(nameof(Port), 9100),
                Ratio = Preferences.Default.Get(nameof(Ratio), 6),
                PositionX = Preferences.Default.Get(nameof(PositionX), 181),
                PositionY = Preferences.Default.Get(nameof(PositionY), 20),
                CaratteriPerRiga = Preferences.Default.Get(nameof(CaratteriPerRiga), 20),
                PositionYRiga1 = Preferences.Default.Get(nameof(PositionYRiga1), 270),
                PositionYRiga2 = Preferences.Default.Get(nameof(PositionYRiga2), 300),
                PositionXTesto = Preferences.Default.Get(nameof(PositionXTesto), 300)
            };
        }
    }

}