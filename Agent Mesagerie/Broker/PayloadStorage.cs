using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using Common;

namespace Broker
{
     class PayloadStorage
     {
          private static ConcurrentQueue<Payload> _payloadQueue;
          public static bool IsEmpty { get { return _payloadQueue.IsEmpty; } }
          static PayloadStorage()
          {
               _payloadQueue = new ConcurrentQueue<Payload>();
          }

          public static void Add(Payload payload)
          {
               _payloadQueue.Enqueue(payload);
          }
          public static Payload GetNext()
          {
               Payload payload = null;
               _payloadQueue.TryDequeue(out payload);
               return payload;
          }
     }
}
