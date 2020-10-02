using Common;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Publisher
{
     class Program
     {
          static void Main(string[] args)
          {
               Console.WriteLine("Publisher.");

               PublisherSocket publisherSocket = new PublisherSocket();
               publisherSocket.Connect(Settings.BROKER_IP, Settings.BROKER_PORT);

               if (publisherSocket.IsConnected)
               {
                    //string mainTopic;
                    //Console.Write("Enter the topic:");
                    //mainTopic = Console.ReadLine().ToLower();

                    while (true)
                    {
                         Payload payload = new Payload();

                         Console.Write("Enter the topic:");
                         payload.Topic = Console.ReadLine().ToLower();
                         //payload.Topic = mainTopic;

                         Console.Write("Enter the message:");
                         payload.Message = Console.ReadLine();

                         var payloadString = JsonConvert.SerializeObject(payload);
                         byte[] data = Encoding.UTF8.GetBytes(payloadString);

                         publisherSocket.Send(data);
                    }
               }

               Console.ReadLine();
          }
     }
}
