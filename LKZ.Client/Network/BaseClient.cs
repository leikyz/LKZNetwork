using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.Network.Client.Network
{
    public class BaseClient
    {
        private TcpClient client = null;
        private NetworkStream stream = null;
        private int id;

        // Event delegate for data received
        public delegate void DataReceivedEventHandler(object sender, string message);

        // Event for data received
        public event DataReceivedEventHandler OnDataReceived;

        public BaseClient(int id)
        {
            this.id = id;

            // Subscribe to the OnDataReceived event
            OnDataReceived += HandleDataReceived;
        }

        public void Connect(string ipAddress, int port)
        {
            client = new TcpClient(ipAddress, port);
            stream = client.GetStream();
            Console.WriteLine("Connected to the server.");

            // Start receiving data in a separate task
            Task.Run(() => ReceiveMessages());
        }

        public void Disconnect()
        {
            stream.Close();
            client.Close();
            Console.WriteLine("Client connection closed.");
        }

        // Send a message to the server
        public void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Sent to server: " + message);
        }

        // Receive messages from the server
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
                    Console.WriteLine("Received from server: " + responseMessage);

                    // Raise the OnDataReceived event
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
            string fullMessage = $"{clientId}|{eventName}|{paramStr}";

            byte[] data = Encoding.ASCII.GetBytes(fullMessage);
            stream.Write(data, 0, data.Length);
        }


        // Method to handle the data received event
        private void HandleDataReceived(object sender, string message)
        {
            Console.WriteLine("Data received: " + message);
        }
    }
}
