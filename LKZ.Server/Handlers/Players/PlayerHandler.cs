using LKZ.Server.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LKZ.Server.Handlers.Players
{
    static public class PlayerHandler
    {
        static public void HandlePlayerCreatedMessage(string[] parameters)
        {

            int sendId = Int32.Parse(parameters[1]);
            Console.WriteLine("test");
            BaseServer.TriggerClientEvent(1, "PlayerCreatedMessage");
           // BaseServer.TriggerClientsEvent(sendId, parameters[0], parameters[1]);
        }

    }
}