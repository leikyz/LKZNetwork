using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LKZ.Network.Common.Events
{

    // crédit : Leikyz
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

            // Vérification pour éviter des appels d'événements invalides
            if (content.Length < 2)
            {
                Console.WriteLine("Invalid event call"); // Utilisation de Debug.Log au lieu de Console.WriteLine
                return;
            }

            // Récupérer le nom de l'événement et les arguments
            string eventName = content[0];
            string[] args = content.Skip(1).ToArray();

            // Appeler toutes les fonctions enregistrées pour cet événement
            if (events.ContainsKey(eventName))
            {
                foreach (var function in events[eventName])
                {
                    function.Invoke(args);
                }
            }
        }

        public static void Trigger(string clientId, string name, params string[] args)
        {
            TriggerRaw(Serialize(name, clientId, args)); // Event name comes first
        }

        public static string Serialize(string eventName, string clientId, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                string paramStr = string.Join("&", parameters);
                return $"{eventName}|{clientId}|{paramStr}~";
            }
            return $"{eventName}|{clientId}|~";
        }

        public static bool ValidateParameters(string[] parameters, int expectedCount)
        {
            return parameters.Length == expectedCount;
        }


        public static string[] Deserialize(string message)
        {
            var parts = message.Split('|');
            if (parts.Length > 1)
            {
                var eventName = parts[0]; // Get the event name
                var clientId = parts[1]; // Get the client ID
                var parameters = (parts.Length > 2 && !string.IsNullOrWhiteSpace(parts[2]))
                                  ? parts[2].Split('&')
                                  : new string[] { }; // Handle empty parameters

                var result = new string[2 + parameters.Length];
                result[0] = eventName; // Store event name
                result[1] = clientId; // Store client ID
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
