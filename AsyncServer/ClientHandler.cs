using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AsyncServer
{
    class ClientHandler
    {
        TcpClient ClientSocket;
        bool ContinueProcess = false;
        Thread ClientThread;

        public ClientHandler(TcpClient ClientSocket)
        {
            this.ClientSocket = ClientSocket;
        }

        public ClientHandler(Socket handler)
        {
        }

        public void Start()
        {
            ContinueProcess = true;
            ClientThread = new Thread(new ThreadStart(Process));
            ClientThread.Start();
        }

        private void Process()
        {

            // Incoming data from the client.
            string data = null;

            // Data buffer for incoming data.
            byte[] bytes;

            if (ClientSocket != null)
            {
                NetworkStream networkStream = ClientSocket.GetStream();
                ClientSocket.ReceiveTimeout = 100; // 1000 miliseconds

                while (ContinueProcess)
                {
                    bytes = new byte[ClientSocket.ReceiveBufferSize];
                    try
                    {
                        int BytesRead = networkStream.Read(bytes, 0, (int)ClientSocket.ReceiveBufferSize);
                        if (BytesRead > 0)
                        {
                            data = Encoding.ASCII.GetString(bytes, 0, BytesRead);

                            // Show the data on the console.
                            Console.WriteLine("Text received : {0}", data);

                            // Echo the data back to the client.
                            byte[] sendBytes = Encoding.ASCII.GetBytes(data);
                            networkStream.Write(sendBytes, 0, sendBytes.Length);

                            if (data == "quit") break;

                        }
                    }
                    catch (System.IO.IOException) { } // Timeout
                    catch (SocketException)
                    {
                        Console.WriteLine("Conection is broken!");
                        break;
                    }
                    Thread.Sleep(200);
                } // while ( ContinueProcess )
                networkStream.Close();
                ClientSocket.Close();
            }
        }  // Process()

        public void Stop()
        {
            ContinueProcess = false;
            if (ClientThread != null && ClientThread.IsAlive)
                ClientThread.Join();
        }

        public bool Alive
        {
            get
            {
                return (ClientThread != null && ClientThread.IsAlive);
            }
        }
    }
}
