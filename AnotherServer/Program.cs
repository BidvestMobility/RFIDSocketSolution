using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading;
using AnotherServer;



public class SynchronousSocketListener
{

    private const int portNum = 10116;

    public static void StartListening()
    {

        ClientService ClientTask;

        // Client Connections Pool
        ClientConnectionPool ConnectionPool = new ClientConnectionPool();

        // Client Task to handle client requests
        ClientTask = new ClientService(ConnectionPool);

        ClientTask.Start();

        TcpListener listener = new TcpListener(portNum);
        try
        {
            listener.Start();

            int ClientLimit = 3; // Number of allowed clients
            int ClientNbr = 0;

            // Start listening for connections.
            Console.WriteLine("Waiting for a connection...");
            while (ClientLimit > 0)
            {

                TcpClient handler = listener.AcceptTcpClient();

                if (handler != null)
                {
                    Console.WriteLine("Client#{0} accepted!", ++ClientNbr);

                    // An incoming connection needs to be processed.
                    ConnectionPool.Enqueue(new ClientHandler(handler));

                    --ClientLimit;
                }
                else
                    break;
            }
            listener.Stop();

            // Stop client requests handling
            ClientTask.Stop();


        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
    }


    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}




