using ZXing.Net.Maui;

namespace QRBarcodeScannerApp.Pages;

public partial class ScanPage : ContentPage
{
	public ScanPage(BarcodeFormat formats)
	{
		InitializeComponent();
		barcodeReader.Options = new BarcodeReaderOptions { Formats = formats, AutoRotate = true, Multiple = false, TryHarder = true };
		barcodeReader.IsDetecting = true;
    }
}