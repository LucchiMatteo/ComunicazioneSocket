using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
//aggiunta
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ComunicazioneSocket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IPEndPoint localendpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 56000);

            Thread t1 = new Thread(new ParameterizedThreadStart(SocketReceive));

            t1.Start(localendpoint);


        }

        public async void SocketReceive (object sourceEndPoint)
        {
            IPEndPoint sourceEP = (IPEndPoint)sourceEndPoint;

            Socket t = new Socket(sourceEP.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            t.Bind(sourceEP);

            Byte[] byteRicevuti = new byte[256];
            string message = "";

            int bytes = 0;

            await Task.Run(() =>
            {
                while (true)
                {
                    if(t.Available>0)
                    {
                        message = "";
                        bytes = t.Receive(byteRicevuti, byteRicevuti.Length, 0);
                        message = message + Encoding.ASCII.GetString(byteRicevuti, 0, bytes);

                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            lblRicezione.Content = message;
                        }));
                    }


                }

            });
        }

        private void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ipDest = IPAddress.Parse(txtIpAdd.Text);
            int portDest = int.Parse(txtDestPort.Text);

            IPEndPoint remoteEndPoint = new IPEndPoint(ipDest, portDest);

            Socket s = new Socket(ipDest.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            Byte[] byteInviati = Encoding.ASCII.GetBytes(txtMsg.Text);

            s.SendTo(byteInviati, remoteEndPoint);
        }
    }
}
