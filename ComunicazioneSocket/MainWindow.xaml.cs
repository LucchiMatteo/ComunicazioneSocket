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
//Aggiunta.
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
        /// <summary>
        /// Programmazione per la comunicazione fra Socket.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //Rappresenta un punto di rete sotto forma di un indirizzo IP e di un numero di porta in questo caso l'ip local.
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 56000);

            //Thread.
            Thread t1 = new Thread(new ParameterizedThreadStart(SocketReceive));

            //Si fa startare il Thread con al suo interno il localEndPoint.
            t1.Start(localEndPoint);
        }

        public async void SocketReceive (object sourceEndPoint)
        {
            //Rappresenta un punto di rete sotto forma di un indirizzo IP e di un numero di porta in questo caso l'ip del mittente.
            IPEndPoint sourceEP = (IPEndPoint)sourceEndPoint;

            //Socket.
            Socket t = new Socket(sourceEP.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            //Instaura la connessione.
            t.Bind(sourceEP);

            //Byte ricevuti.
            Byte[] byteRicevuti = new byte[256];
            string message = "";

            int bytes = 0;

            //Metodo che permette a l'altro processo di andare normalmente.
            await Task.Run(() =>
            {
                while (true)
                {
                    //Se il Thread è maggiore di 0 il messaggio verrà inviato.
                    if (t.Available>0)
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
            //IPAddress del destinatario.
            IPAddress ipDest = IPAddress.Parse(txtIpAdd.Text);
            int portDest = int.Parse(txtDestPort.Text);

            //Punto remoto.
            IPEndPoint remoteEndPoint = new IPEndPoint(ipDest, portDest);

            //Socket.
            Socket s = new Socket(ipDest.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            //Byte.
            Byte[] byteInviati = Encoding.ASCII.GetBytes(txtMsg.Text);

            s.SendTo(byteInviati, remoteEndPoint);
        }
    }
}
