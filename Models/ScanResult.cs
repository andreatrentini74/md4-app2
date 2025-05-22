using QRBarcodeScannerApp.Services;

namespace QRBarcodeScannerApp.Models
{
    public class ScanResult
    {
        public bool QrCodeOnly { get; set; } = false;
        public string QrCode { get; set; }
        public string BarCode { get; set; }
        public string ConcatenatedResult(AppSettings settings)
        {
            string _ratio = settings.Ratio.ToString().PadLeft(2, '0');
            // posizione X e Y
            string _positionX = settings.PositionX.ToString().PadLeft(4, '0');
            string _positionY = settings.PositionY.ToString().PadLeft(4, '0');

            // solo qrcode o codici uniti
            string newCode = this.QrCodeOnly ? this.QrCode : this.QrCode + " " + this.BarCode;
            string newCodeLength = newCode.Length.ToString().PadLeft(4, '0');

            string _QrCode1 = this.QrCode.Substring(0, Math.Min(settings.CaratteriPerRiga, this.QrCode.Length));
            string _QrCode2 = this.QrCode.Length > settings.CaratteriPerRiga ? this.QrCode.Substring(settings.CaratteriPerRiga) : string.Empty;
            _QrCode2 = _QrCode2.Substring(0, Math.Min(settings.CaratteriPerRiga, _QrCode2.Length));

            return "{D0420,0700,0400|}\n" +
           "{C|}\n" +
           "{XB00;" + _positionX + "," + _positionY + ",T,L," + _ratio + ",A,0,M2;01|}\n" +
           $"{{PV00;{settings.PositionXTesto:0000},{settings.PositionYRiga1:0000},0030,0030,B,00,B,P2=" + _QrCode1 + "|}\n" +
           $"{{PV00;{settings.PositionXTesto:0000},{settings.PositionYRiga2:0000},0030,0030,B,00,B,P2=" + _QrCode2 + "|}\n" +
           "{RB;^<" + newCodeLength + "^<" + newCode + "|}\n" +
           "{XS;I,0001,0002C6200|}";
        }
    }
}
