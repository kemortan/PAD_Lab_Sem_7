using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broker
{
     class PayloadHandler
     {
          public static void Handle(byte[] payloadBytes, ConnectionInfo connectionInfo)
          {
               var payloadString = Encoding.UTF8.GetString(payloadBytes);
               Console.WriteLine(payloadString);
          }
     }
}
