using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Network;

namespace SEP3Tier1App.Network
{
    public class NetworkSocketImpl : INetworkSocket
    {
        public Delegating _delegating { get; set; }
        private NetworkStream _networkStream;
        private string Username;


        public NetworkSocketImpl()
        {
            Console.WriteLine("constructor");
            _delegating = new Delegating();
            _networkStream = NetworkStream();
            Thread thread = new Thread(() => ListenToServer());
            thread.Start();
        }


        public void SendUsername()
        {
            Console.WriteLine(Username);
            var bytes = Encoding.ASCII.GetBytes(Username);
            _networkStream.Write(bytes, 0, bytes.Length);
        }

        public void SendMessage(PrivateMessage message)
        {
            Request request = new Request()
            {
                o = JsonSerializer.Serialize(message),
                requestOperation = RequestOperationEnum.SENDMESSAGE
            };
            var serialize = JsonSerializer.Serialize(request);
            var bytes = Encoding.ASCII.GetBytes(serialize);
            _networkStream.Write(bytes);
        }
        
        public void closeConnection()
        {
            Console.WriteLine("VI LUKKER STREAM");
            _networkStream.Close();
        }


        private void ListenToServer()
        {
            while (true)
            {
                try
                {
                    byte[] dataFromClient = new byte[1024];
                    _networkStream.Read(dataFromClient, 0, dataFromClient.Length);
                    var trimEmptyBytes = TrimEmptyBytes(dataFromClient);
                    string s = Encoding.ASCII.GetString(trimEmptyBytes, 0, trimEmptyBytes.Length);
                    Request request = JsonSerializer.Deserialize<Request>(s);
                    switch (request.requestOperation)
                    {
                        case RequestOperationEnum.GETCONNECTIONS:
                        {
                            Console.WriteLine("vi er i switch på tier1");
                            Console.WriteLine(request.o);
                            _delegating.GetConnectionsFromNetwork?.Invoke(request);
                            IList<string> Images = new Collection<string>();
                            int numberOfImages = JsonSerializer.Deserialize<IList<Connections>>(request.o.ToString())
                                .Count;
                            for (int i = 0; i < numberOfImages; i++)
                            {
                                byte[] newArray = new byte[1024 * 1024];
                                _networkStream.Read(newArray, 0, newArray.Length);
                                var trimEmptyBytes2 = TrimEmptyBytes(newArray);
                                string s2 = Encoding.ASCII.GetString(trimEmptyBytes2, 0, trimEmptyBytes2.Length);
                                Images.Add(s2);
                            }

                            _delegating.ImagesFromNetwork?.Invoke(Images);
                            break;
                        }
                        case RequestOperationEnum.NOTIFYABOUTMESSAGES:
                        {
                            Console.WriteLine("jeg sender en notify");
                            _delegating.NotfifyAboutNewMessage?.Invoke();
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
            }

            Console.WriteLine("Vi er lige her før close");
            _networkStream.Close();
        }


        private byte[] TrimEmptyBytes(byte[] array)
        {
            int i = array.Length - 1;
            while (array[i] == 0)
            {
                --i;
            }

            byte[] bar = new byte[i + 1];
            Array.Copy(array, bar, i + 1);
            return bar;
        }


        public async Task getConnections(string username)
        {
            string request = JsonSerializer.Serialize(new Request()
            {
                Username = username,
                requestOperation = RequestOperationEnum.GETCONNECTIONS
            });

            byte[] bytes = Encoding.ASCII.GetBytes(request);
            _networkStream.Write(bytes, 0, bytes.Length);
        }


        private static NetworkStream NetworkStream()
        {
            NetworkStream stream = null;

            try
            {
                TcpClient tcpClient = new TcpClient("127.0.0.1", 4500);
                stream = tcpClient.GetStream();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return stream;
        }

        public Delegating getDelegating()
        {
            return _delegating;
        }

        public void setUsername(string username)
        {
            Username = username;
        }
    }
}