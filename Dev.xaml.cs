using System.Windows.Controls;
using System.Windows.Media;
using System.Net.Sockets;
using SocketIOClient;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System;

namespace Nexa
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Dev : UserControl
    {
        public static Dev dev;
        public UserControl registerView => new UserControl();
        public UserControl chatView => new Chat();

        public UserControl loginView { get; private set; }



        public Dev()
        {
            InitializeComponent();
            dev = this;
        }

        private bool isWSCConnected = false;
        private SocketIO client;
        private Color error = Color.FromRgb(255, 0, 0);
        private Color ok = Color.FromRgb(0, 255, 0);
        private Color eh = Color.FromRgb(255, 192, 0);

        async private void WSCBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!isWSCConnected)
            {
                // Connect to the WebSocket
                client = new SocketIO("ws://fi.dynamichost.cc:20100");
                client.OnConnected += async (sender, e) =>
                {
                    await client.EmitAsync("hi", "Developer has opened an WS Connection using the Windows App.");
                    Dispatcher.Invoke(() =>
                    {
                        WSCStatus.Foreground = new System.Windows.Media.SolidColorBrush(ok);
                        WSCStatus.Text = "Success";
                        WSCBtn.Content = "Close Session";
                    });
                    return;
                };

                client.OnError += (sender, e) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        WSCStatus.Foreground = new System.Windows.Media.SolidColorBrush(error);
                        WSCStatus.Text = "Error (idk)";
                    });
                    return;
                };

                client.OnDisconnected += async (sender, e) =>
                {
                    await client.EmitAsync("hi", "Developer has closed the WS Connection from the Windows App.");
                    Dispatcher.Invoke(() =>
                    {
                        WSCStatus.Foreground = new System.Windows.Media.SolidColorBrush(eh);
                        WSCStatus.Text = "Session Closed";
                        WSCBtn.Content = "Test WS Connect";
                    });
                    return;
                };
                await client.ConnectAsync();
                isWSCConnected = true;
            }
            else
            {
                // Disconnect from the WebSocket
                await client.DisconnectAsync();
                isWSCConnected = false;
            }
        }


        private void PingBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PingStatus.Text = "";

            Ping ping = new Ping();

            try
            {
                PingReply reply = ping.Send("fi.dynamichost.cc", 20100);

                if (reply.Status == IPStatus.Success)
                {
                    PingStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    PingStatus.Text = $"Succeeded {reply.RoundtripTime} ms";
                }
                else
                {
                    PingStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    PingStatus.Text = $"Failed with error: {reply.Status.ToString()}";
                }
            }
            catch (Exception ex)
            {
                PingStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                PingStatus.Text = $"Failed with ex: {ex.Message}";
            }
        }


    }
}
