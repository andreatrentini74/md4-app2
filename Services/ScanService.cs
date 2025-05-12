using System.Net.Http.Json;
using System.Text;
using QRBarcodeScannerApp.Models;

namespace QRBarcodeScannerApp.Services
{
    public class ScanService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;

        public ScanService(AppSettings settings)
        {
            _settings = settings;
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public async Task<bool> SendScanResultAsync(string scanResult)
        {
            try
            {
                UriBuilder builder = new UriBuilder();
                builder.Scheme = "http";
                builder.Host = _settings.IPAddress;
                builder.Port = _settings.Port;
                var response = await _httpClient.PostAsync(builder.Uri, new StringContent(scanResult, Encoding.UTF8));
                return response.IsSuccessStatusCode;
            }
            catch
            {
                throw;
            }
        }
    }
}