using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using Common;

namespace Subscriber
{
     class SubscriberSocket
     {
          private Socket _socket;
          private string _topic;

          public SubscriberSocket(string topic)
          {
               _topic = topic;
               _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
          }

          public void Connect(string ipAddress, int port)
          {
               _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectedCallback, null);
               Console.WriteLine("Waiting for connection");
          }
          private void ConnectedCallback(IAsyncResult asyncResult)
          {
               if (_socket.Connected)
               {
                    Console.WriteLine("Subscriber conected to broker");
                    Subscribe();
                    StartReceive();
               }
               else
               {
                    Console.WriteLine("Error: Subscriber couldn't connect to broker.");
               }
          }
          private void Subscribe()
          {
               var data = Encoding.UTF8.GetBytes(Settings.SUBSCRIBE_COMM + _topic);
               Send(data);
          }
          private void StartReceive()
          {
               ConnectionInfo connection = new ConnectionInfo();
               connection.Socket = _socket;
               _socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None,
                    ReceiveCallback, connection);
          }
          private void ReceiveCallback(IAsyncResult asyncResult)
          {
               ConnectionInfo connectionInfo = asyncResult.AsyncState as ConnectionInfo;

               try
               {
                    SocketError response;
                    int buffSize = _socket.EndReceive(asyncResult, out response);

                    if (response == SocketError.Success)
                    {
                         byte[] payloadBytes = new byte[buffSize];
                         Array.Copy(connectionInfo.Data, payloadBytes, payloadBytes.Length);

                         PayloadHandler.Handle(payloadBytes);
                    }
               }catch(Exception e)
               {
                    Console.WriteLine($"Can't receive data from broker: {e.Message}");
               }
               finally
               {
                    try
                    {
                         _socket.BeginReceive(connectionInfo.Data, 0, connectionInfo.Data.Length, SocketFlags.None,
                         ReceiveCallback, connectionInfo);
                    }
                    catch(Exception e)
                    {
                         Console.WriteLine($"{e.Message}");
                         connectionInfo.Socket.Close();
                    }
               }
          }
          private void Send(byte[] data)
          {
               try
               {
                    _socket.Send(data);
               }
               catch(Exception e)
               {
                    Console.WriteLine($"Couldn't send data: {e.Message}");
               }
          }
          
     }
}
