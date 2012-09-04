using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ChatHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = new Uri(ConfigurationManager.AppSettings["addr"]);
            ServiceHost host = new ServiceHost(typeof(ChatLib.ChatService), uri);
            host.Open();
            Console.WriteLine("Chat service Host now listening on Endpoint {0}",
            uri.ToString());
            Console.WriteLine("Press ENTER to stop chat service...");
            Console.ReadLine();
            host.Abort();
            host.Close(); 

        }
    }
}
