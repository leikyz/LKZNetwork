using System;
using System.Collections.Generic;
using System.Linq;

namespace LKZ.Network.Common.Events
{
    public static class EventManager
    {
        private static Dictionary<string, List<Action<string[]>>> events = new Dictionary<string, List<Action<string[]>>>();

        public static void RegisterEvent(string name, Action<string[]> function)
        {
            if (!events.ContainsKey(name))
            {
                events[name] = new List<Action<string[]>>();
            }
            events[name].Add(function);
        }

        public static void TriggerRaw(string message)
        {
            string[] content = Deserialize(message);
            if (content.Length < 2)
            {
                Console.WriteLine("Invalid event call");
                return;
            }

            string clientId = content[0]; // Get the client ID
            string eventName = content[1]; // Get the event name

            if (events.ContainsKey(eventName))
            {
                string[] args = content.Skip(2).ToArray(); // Skip the first two elements (client ID and event name)
                foreach (var function in events[eventName])
                {
                    function.Invoke(args);
                }
            }
        }

        public static void Trigger(string clientId, string name, string[] args)
        {
            TriggerRaw(Serialize(clientId, name, args));
        }

        public static string Serialize(string clientId, string eventName, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                string paramStr = string.Join(",", parameters);
                return $"{clientId}|{eventName}|{paramStr}";
            }
            return $"{clientId}|{eventName}|";
        }

        public static string[] Deserialize(string message)
        {
            var parts = message.Split('|');
            if (parts.Length > 0)
            {
                var clientId = parts[0]; // Get the client ID
                var command = parts[1]; // Get the event name
                var parameters = parts.Length > 2 ? parts[2].Split(',') : new string[] { };

                var result = new string[1 + parameters.Length + 1];
                result[0] = clientId; // Store client ID
                result[1] = command; // Store event name
                for (int i = 0; i < parameters.Length; i++)
                {
                    result[i + 2] = parameters[i]; // Store parameters
                }
                return result;
            }

            return new string[] { };
        }
    }
}
