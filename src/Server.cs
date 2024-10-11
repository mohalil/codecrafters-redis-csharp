using System.Net;
using System.Net.Sockets;
using System.Text;

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

// Uncomment this block to pass the first stage
TcpListener server = new TcpListener(IPAddress.Any, 6379);
server.Start();
//server.AcceptSocket(); // wait for client
Console.WriteLine("Server started, waiting for a client...");
while (true)
{
    var clientSocket = server.AcceptSocket(); // wait for client
    Console.WriteLine("Client connected.");
    await HandleClient(clientSocket);
}
static async Task HandleClient(Socket clientSocket)
{
    var buffer = new byte[1024];
    int bytesRead = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
    Console.WriteLine($"Received: {receivedMessage}");
    var response = Encoding.UTF8.GetBytes("+PONG\r\n");
    await clientSocket.SendAsync(response, SocketFlags.None);
    clientSocket.Shutdown(SocketShutdown.Both);
    clientSocket.Close();
}