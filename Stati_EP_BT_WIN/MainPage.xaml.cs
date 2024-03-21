using Stati_EP_BT_WIN.StatiBLE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



namespace Stati_EP_BT_WIN
{
    public sealed partial class MainPage : Page
    {
        StatiBLEManager StatiBLE = null;
        public MainPage()
        {
            this.InitializeComponent();
            StatiBLE = new StatiBLEManager(Log);
        }

        private async void btn_start_Click(object sender, RoutedEventArgs e)
        {
            await StatiBLE.Run();
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            StatiBLE.Stop();
        }

        public async void Log(string message)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                logBox.Text += message + "\n";
            });
            
        }
       
    }
}
