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
            int x = Int32.Parse(parameters[2]);
            int y = Int32.Parse(parameters[3]);
            int z = Int32.Parse(parameters[4]);

            //if (sendId == 1)
            //    BaseServer.TriggerClientEvent(2, "PlayerCreatedMessage", x, y, z);
            //else
            //    BaseServer.TriggerClientEvent(1, "PlayerCreatedMessage", x, y, z);

            BaseServer.TriggerClientsWithoutSenderEvent(sendId, "PlayerCreatedMessage", x, y, z);
            // BaseServer.TriggerClientsEvent(sendId, parameters[0], parameters[1]);
        }

    }
}