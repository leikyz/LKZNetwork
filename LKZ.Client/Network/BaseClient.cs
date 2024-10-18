using LKZ.Server.Network;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.Network.Client.Network
{
    public class BaseClient
    {
        private const int TimeBetweenMessage = 40; // ms

        private TcpClient client = null;
        private NetworkStream stream = null;
        private int id;
     

        public delegate void DataReceivedEventHandler(object sender, string message);

        public event DataReceivedEventHandler OnDataReceived;

        public BaseClient(int id)
        {
            this.id = id;
            OnDataReceived += HandleDataReceived;
        }

        public int Id { get { return id; } }

        public void Connect(string ipAddress, int port)
        {
            client = new TcpClient(ipAddress, port);
            stream = client.GetStream();
            Console.WriteLine("Connected to the server.");

            Task.Run(() => ReceiveMessages());
        }

        public void Disconnect()
        {
            stream.Close();
            client.Close();
            Console.WriteLine("Client connection closed.");
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Sent to server: " + message);
        }

        private void ReceiveMessages()
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        break; // Connection closed by server
                    }

                    string responseMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    OnDataReceived?.Invoke(this, responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error receiving message: " + ex.Message);
            }
            finally
            {
                Disconnect(); // Ensure the connection is closed on exit
            }
        }

        public void TriggerServerEvent(int clientId, string eventName, params object[] parameters)
        {
            string paramStr = string.Join(",", parameters);
            string fullMessage = $"{eventName}|{clientId}|{paramStr}"; // Change the order

            byte[] data = Encoding.ASCII.GetBytes(fullMessage);
            stream.Write(data, 0, data.Length);
            Thread.Sleep(TimeBetweenMessage);
        }

        private void HandleDataReceived(object sender, string message)
        {
            Console.WriteLine($"Message received ({message})");
        }
    }
}
