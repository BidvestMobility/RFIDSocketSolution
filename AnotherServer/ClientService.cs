﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AnotherServer
{
    class ClientService
    {

        const int NUM_OF_THREAD = 10;

        private ClientConnectionPool ConnectionPool;
        private bool ContinueProcess = false;
        private Thread[] ThreadTask = new Thread[NUM_OF_THREAD];

        public ClientService(ClientConnectionPool ConnectionPool)
        {
            this.ConnectionPool = ConnectionPool;
        }

        public void Start()
        {
            ContinueProcess = true;
            // Start threads to handle Client Task
            for (int i = 0; i < ThreadTask.Length; i++)
            {
                ThreadTask[i] = new Thread(new ThreadStart(this.Process));
                ThreadTask[i].Start();
            }
        }

        private void Process()
        {
            while (ContinueProcess)
            {

                ClientHandler client = null;
                lock (ConnectionPool.SyncRoot)
                {
                    if (ConnectionPool.Count > 0)
                        client = ConnectionPool.Dequeue();
                }
                if (client != null)
                {
                    client.Process(); // Provoke client
                                      // if client still connect, schedufor later processingle it 
                    if (client.Alive)
                        ConnectionPool.Enqueue(client);
                }

                Thread.Sleep(100);
            }
        }

        public void Stop()
        {
            ContinueProcess = false;
            for (int i = 0; i < ThreadTask.Length; i++)
            {
                if (ThreadTask[i] != null && ThreadTask[i].IsAlive)
                    ThreadTask[i].Join();
            }

            // Close all client connections
            while (ConnectionPool.Count > 0)
            {
                ClientHandler client = ConnectionPool.Dequeue();
                client.Close();
                Console.WriteLine("Client connection is closed!");
            }
        }

    } 
}
