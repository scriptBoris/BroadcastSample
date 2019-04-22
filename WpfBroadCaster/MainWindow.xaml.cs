using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfBroadCaster
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BroadCaster _broadCaster;
        public MainWindow()
        {
            InitializeComponent();
            _broadCaster = new BroadCaster(8008);
        }

        public void WriteText(string msg)
        {
            TextBoxConsole.Text += msg + Environment.NewLine;
        }

        protected override void OnClosed(EventArgs e)
        {
            _broadCaster.Stop();
            base.OnClosed(e);
        }

        private void ButtonStartClick(object sender, RoutedEventArgs e)
        {
            if (_broadCaster.IsStarting == false)
                _broadCaster.Start();
        }

        private void ButtonEndClick(object sender, RoutedEventArgs e)
        {
            if (_broadCaster.IsStarting)
                _broadCaster.Stop();
        }
    }

    internal class BroadCaster
    {
        private readonly UdpClient _client;
        private readonly Thread _loop;
        private bool _isEnabled = false;
        private MainWindow _window;

        public bool IsStarting => _isEnabled;

        public BroadCaster(int port)
        {
            _client = new UdpClient();
            _client.Client.Bind(new IPEndPoint(IPAddress.Any, port));
            _loop = new Thread(Loop) { IsBackground = true };

            _window = (MainWindow)Application.Current.MainWindow;
        }

        private void Loop()
        {
            var clientIp = new IPEndPoint(0, 0);
            while (_isEnabled)
            {
                // Get message from clients
                byte[] recvBuffer = _client.Receive(ref clientIp);
                string clientText = Encoding.UTF8.GetString(recvBuffer);

                // Write text in GUI
                _window.TextBoxConsole.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate { _window.TextBoxConsole.Text += clientText;});

                // Response
                string echoMsg = "Hello client, im server!";
                byte[] sendBytes4 = Encoding.ASCII.GetBytes(echoMsg);
                _client.Send(sendBytes4, sendBytes4.Length, clientIp);
            }
        }

        public void Start()
        {
            if (_isEnabled == false)
            {
                _isEnabled = true;
                _window.WriteText("Server is listen");
                _loop.Start();
            }
        }

        public void Stop()
        {
            if (_isEnabled)
            {
                _window.WriteText("Server has stop listen");
                _isEnabled = false;
            }
        }
    }
}
