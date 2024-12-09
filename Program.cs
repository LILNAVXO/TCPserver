using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TCPserver
{
  private TcpListener tcpListener;
  private bool isrunning;

  public TCPserver(string ipaddress, int port)
  {
    tcpListener = new TcpListener(IPAddress.Parse(ipaddress), port);
  }

  public void Start()
  {
    tcpListener.Start();
    isrunning = true;
    Console.WriteLine("Server starting............ \n Server Started(200) \n Waiting for Clients to connect........");

    while(isrunning)
    {
      try
      {
        TcpClient client = tcpListener.AcceptTcpClient();
        Console.WriteLine("Client Connected!");

        Thread clientThread = new Thread(() => HandleClient(client));
        clientThread.Start();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error handling client communication" + ex.Message);
      }
    }
  }

  private void HandleClient(TcpClient client)
  {
    NetworkStream stream = client.GetStream();
    byte[] buffer = new byte[1024];
    int bytesRead;

    try
    {
      while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
      {
        string receiveData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Received from Client: " + receiveData);

        string response = "Message Received!";
        byte[] responsebytes = Encoding.ASCII.GetBytes(response);
        stream.Write(responsebytes, 0, responsebytes.Length);
        Console.WriteLine("Sent to Client: " + response);
      }
    }
    catch(Exception ex)
    {
      Console.WriteLine("Couldn't communicate with the client!: " + ex.Message);
    }
    finally
    {
      client.Close();
      Console.WriteLine("Connection Closed!");
    }
  }

  public void Stop()
  {
    isrunning = false;
    tcpListener.Stop();
    Console.WriteLine("Server Stopped");
  }
}

partial class Program
{
    static void Main(string[] args)
    {
        string ipAddress = "127.0.0.1";
        int port = 12345;

        TCPserver server = new TCPserver(ipAddress, port);
        server.Start();
    }
}
