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

            //Oggetto rappresentante del nostro IP locale
            IPEndPoint localendpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 56000);

            Thread t1 = new Thread(new ParameterizedThreadStart(SocketReceive));
            //Thread per istaurare la connessione in ricezione
            t1.Start(localendpoint);
        }

        IPEndPoint _currentSourceSocket;

        public async void SocketReceive (object sourceEndPoint)
        {
            try
            {
                IPEndPoint sourceEP = (IPEndPoint)sourceEndPoint;

                _currentSourceSocket = sourceEP;
                //Creazione del socket per la trasmissione
                Socket t = new Socket(sourceEP.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

                t.Bind(sourceEP);
                Byte[] byteRicevuti = new byte[256];
                string message = "";

                int bytes = 0;
                //Ciclo per l'acconsentire della comunicazione
                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (t.Available > 0)
                        {
                            message = "";
                            bytes = t.Receive(byteRicevuti, byteRicevuti.Length, 0);
                            message = message + Encoding.ASCII.GetString(byteRicevuti, 0, bytes);

                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                txtRicezione.Text = message;
                            }));                            
                        }
                    }                    
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Controlli per l'istaurazione della comunicazione
                IPAddress ipDest = IPAddress.Parse(txtIpAdd.Text);
                int portDest = int.Parse(txtDestPort.Text);
                //Crezione di un EndPoint della comunicazione
                IPEndPoint remoteEndPoint = new IPEndPoint(ipDest, portDest);

                Socket s = new Socket(ipDest.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                //Codifica del testo in byte
                Byte[] byteInviati = Encoding.ASCII.GetBytes(txtMsg.Text);

                s.SendTo(byteInviati, remoteEndPoint);
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERRORE: " + ex.Message, "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSourceSocket_Click(object sender, RoutedEventArgs e)
        {
            lblSourceSocket.Content = "Socket Host Corrente: " + _currentSourceSocket.Address + ":" + _currentSourceSocket.Port;
        }

        #region Morra Cinese
        //I metodi hanno le stesse funzionalità del metodo Invia
        private void btnRock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPAddress ipDest = IPAddress.Parse(txtIpAdd.Text);
                int portDest = int.Parse(txtDestPort.Text);

                IPEndPoint remoteEndPoint = new IPEndPoint(ipDest, portDest);

                Socket s = new Socket(ipDest.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

                Byte[] byteInviati = Encoding.ASCII.GetBytes("     ______" +
                                                          "\n-- - '   ____)" +
                                                          "\n         (_____)" +
                                                          "\n         (_____)" +
                                                          "\n         (____)" +
                                                          "\n-- -.__(___)"+
                                                          "\n   SASSO");

                s.SendTo(byteInviati, remoteEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRORE: " + ex.Message, "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPaper_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPAddress ipDest = IPAddress.Parse(txtIpAdd.Text);
                int portDest = int.Parse(txtDestPort.Text);

                IPEndPoint remoteEndPoint = new IPEndPoint(ipDest, portDest);

                Socket s = new Socket(ipDest.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

                Byte[] byteInviati = Encoding.ASCII.GetBytes("     _______" +
                                                           "\n---'    ____)____" +
                                                           "\n             ______)" +
                                                           "\n            _______)" +
                                                           "\n            _______)" +
                                                           "\n---.__________)" +
                                                           "\n   CARTA");

                s.SendTo(byteInviati, remoteEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRORE: " + ex.Message, "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnScissors_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPAddress ipDest = IPAddress.Parse(txtIpAdd.Text);
                int portDest = int.Parse(txtDestPort.Text);

                IPEndPoint remoteEndPoint = new IPEndPoint(ipDest, portDest);

                Socket s = new Socket(ipDest.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

                Byte[] byteInviati = Encoding.ASCII.GetBytes("     _______" +
                                                             "\n---'    ____)____" +
                                                             "\n             ______)" +
                                                             "\n          _______)" +
                                                             "\n       (____)" +
                                                             "\n-- -._(___)" +
                                                             "\n   FORBICI");

                s.SendTo(byteInviati, remoteEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRORE: " + ex.Message, "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
