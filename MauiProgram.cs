﻿using Microsoft.Extensions.Logging;
using md4.Services;
using md4.ViewModels;
using CommunityToolkit.Maui;
using md4.Pages;
using BarcodeScanning;

namespace md4
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBarcodeScanning()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Configurazione del lettore di barcode
            //builder.Services.ConfigureBarcodeReader(options =>
            //{
            //    // Configurazione generale
            //    options.Default.Formats = BarcodeFormats.All;
            //    options.Default.AutoRotate = true;
            //    options.Default.Multiple = false;

            //    // Configurazione Android
            //    options.Android.UseNewDecoder = true;
            //    options.Android.DisableAutofocus = false;
            //    options.Android.DisableContinuous = false;
            //    options.Android.TryHarder = true;
            //    options.Android.PureBarcode = false;

            //    // Configurazione iOS
            //    options.iOS.UseNative = true;
            //    options.iOS.DisableAutofocus = false;
            //    options.iOS.DisableContinuous = false;
            //});

            // Registra i servizi
            builder.Services.AddSingleton(AppSettings.Load());
            builder.Services.AddSingleton<ScanService>();

            // Registra i ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<ConfigViewModel>();

            // Registra le pagine
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ConfigPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif


            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                HandleException(e.ExceptionObject as Exception);
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                HandleException(e.Exception);
                e.SetObserved();
            };

            return builder.Build();
        }

        private static void HandleException(Exception? ex)
        {
            if (ex == null) return;

            // Visualizza l'errore sulla UI principale
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Errore non gestito",
                        ex.Message,
                        "OK");
                }
            });
        }
    }
}