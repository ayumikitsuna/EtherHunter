using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AndersL.EtherHunter
{
    public partial class BuyWindow : Window
    {
        public BuyWindow()
        {
            InitializeComponent();
        }

        private void Mail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenUrl("mailto:alondstrom@protonmail.com");
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
