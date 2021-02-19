using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SynchronousSocketClient
{

    public static void StartClient()
    {
        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];

        string instruction;

        // Connect to a remote device.  
        try
        {
            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.  
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                sender.Connect(remoteEP);
                Console.WriteLine("Socket connected to {0}",
                sender.RemoteEndPoint.ToString());

                Console.WriteLine("Type 'start' to start scanning. Type 'end' to end scanning.");

                while (true)
                {
                    // Encode the data string into a byte array.  
                    instruction = Console.ReadLine();
                    //if (instruction.Equals("start") == true)
                    //{
                    byte[] msg = Encoding.ASCII.GetBytes(instruction);

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);
                    //}
                    // Clear msg.  
                    //Array.Clear(msg, 0, msg.Length);

                    // if (instruction.Equals("end") == true)
                    //  {
                    // Receive the response from the remote device. 
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine($"{Encoding.ASCII.GetString(bytes, 0, bytesRec)}");
                    // }
                }

                // Release the socket.  
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"ArgumentNullException : {ex.Message}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException : {0}", ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception : {0}", ex.ToString());
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        Console.ReadLine();
    }

    public static int Main(String[] args)
    {
        StartClient();
        return 0;
    }
}