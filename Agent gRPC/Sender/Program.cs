using System;
using Grpc.Net.Client;
using gRPCAgent;
using Common;
using System.Threading.Tasks;

namespace Sender
{
     class Program
     {
          static async Task Main(string[] args)
          {
               Console.WriteLine("Publisher");

               AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
               var channel = GrpcChannel.ForAddress(EndpointsConstants.BrokerAddress);
               var client = new Publisher.PublisherClient(channel);

               while (true)
               {
                    Console.Write("Enter the topic: ");
                    var topic = Console.ReadLine().ToLower();

                    Console.Write("Enter content: ");
                    var content = Console.ReadLine();

                    var request = new PublishRequest() { Topic = topic, Content = content };

                    try
                    {
                         var reply = await client.PublishMessageAsync(request);
                         Console.WriteLine($"Publish reply: {reply.IsSuccess}");
                    }
                    catch(Exception e)
                    {
                         Console.WriteLine($"Error pulishing the message: {e.Message}");
                    }
               }
          }
     }
}
