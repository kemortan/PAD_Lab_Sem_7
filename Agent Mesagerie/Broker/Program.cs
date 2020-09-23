using Common;
using System;

namespace Broker
{
     class Program
     {
          static void Main(string[] args)
          {
               Console.WriteLine("Broker.");

               BrokerSocket socket = new BrokerSocket();
               socket.Start(Settings.BROKER_IP, Settings.BROKER_PORT);

               Console.ReadLine();
          }
     }
}
