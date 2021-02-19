using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SynchronousSocketListener
{

    // Incoming data from the client.  
    public static string data = null;

    public static void StartListening()
    {
        // Data buffer for incoming data.  
        byte[] bytes = new Byte[1024];

        // Establish the local endpoint for the socket.  
        // Dns.GetHostName returns the name of the
        // host running the application.  
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        // Create a TCP/IP socket.  
        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and
        // listen for incoming connections.  
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            Console.WriteLine("Waiting for instruction from client...");

            // Start listening for connections.  
            while (true)
            {
                // Program is suspended while waiting for an incoming connection.  
                Socket handler = listener.Accept();
                data = null;

            while (true)
                {
                    // An incoming connection needs to be processed.  
                    int bytesRec = handler.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    //if (data.IndexOf("<EOF>") > -1)
                    //{
                    //    break;
                    //}
                    //}

                    if (data.Equals("start") == true)
                    {
                        Console.WriteLine("Scanning started...");
                        // Echo the status back to the client.  
                        byte[] msg = Encoding.ASCII.GetBytes("Server started scanning...");
                        handler.Send(msg);
                    }

                    if (data.Equals("end") == true)
                    {
                        Console.WriteLine("Scanning ended...");
                        // Echo the status back to the client.  
                        byte[] msg = Encoding.ASCII.GetBytes("Server stopped scanning...");
                        handler.Send(msg);
                    }


                    //Some code to store scanned data into array

                }
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();
    }

    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}