using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LKZ.Network.Common.Events
{

    // crédit : Leikyz
    public static class EventManager
    {
        private static Dictionary<string, List<Action<BaseClient, string[]>>> events = new Dictionary<string, List<Action<BaseClient, string[]>>>();

        public static void RegisterEvent(string name, Action<BaseClient, string[]> function)
        {
            if (!events.ContainsKey(name))
            {
                events[name] = new List<Action<BaseClient, string[]>>();
            }
            events[name].Add(function);
        }

        public static void TriggerRaw(BaseClient client, string message)
        {
            string[] content = Deserialize(message);

            if (content.Length < 1)
            {
                Console.WriteLine("Invalid event call"); 
                return;
            }
            string eventName = content[0];
            string[] args = content.Skip(1).ToArray();

            if (events.ContainsKey(eventName))
            {
                foreach (var function in events[eventName])
                {
                    function.Invoke(client,args);
                }
            }
        }

        public static string Serialize(string eventName, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                string paramStr = string.Join("&", parameters);
                return $"{eventName}|{paramStr}~";
            }
            return $"{eventName}|~";
        }

        public static string[] Deserialize(string message)
        {
            var parts = message.Split('|');

            if (parts.Length >= 2)
            {
                var eventName = parts[0];

                if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]))
                {
                    var parameters = parts[1].Split('&');
                    var result = new string[1 + parameters.Length];
                    result[0] = eventName; 

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        result[i + 1] = parameters[i]; 
                    }
                    return result;
                }
                return new string[] { eventName };
            }
            return new string[] { };
        }


        public static bool ValidateParameters(string[] parameters, int expectedCount)
        {
            return parameters.Length == expectedCount;
        }
    }
}
