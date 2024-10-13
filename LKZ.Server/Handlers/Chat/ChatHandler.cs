using LKZ.Server.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LKZ.Server.Handlers.Chat
{
    static public class ChatHandler
    {
        static public void HandleChatMessageMessage(string[] parameters)
        {
            TcpClient sender = BaseServer.GetTcpClient(Int32.Parse(parameters[0]));
            TcpClient receiver = BaseServer.GetTcpClient(Int32.Parse(parameters[1]));

            //BaseServer.ListClients();

            byte[] data = Encoding.ASCII.GetBytes(parameters[2]);
            receiver.GetStream().Write(data, 0, data.Length);

            //NetworkStream stream = receiver.GetStream();


            //byte[] data = Encoding.ASCII.GetBytes(parameters[2]);
            //stream.Write(data, 0, data.Length);
        }

    }
}