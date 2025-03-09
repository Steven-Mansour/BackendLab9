// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.SignalR.Client;

class Program
{
    static async Task Main(string[] args)
    {
        string clientID = "client2";
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5250/chat")
            .Build();

        connection.On<string, string>("ReceiveMessage", (senderId, message) =>
        {
            Console.WriteLine($"{senderId}: {message}");
        });

        await connection.StartAsync();
        await connection.InvokeAsync("JoinChat", clientID);
        Console.WriteLine("Connected to chat. Type your messages...");

        string receiverID = "client1";
        while (true)
        {
            Console.Write($"Current Client ID: {clientID} | Enter new Client ID (or press Enter to keep current): ");
            var newClientID = Console.ReadLine();
            if (!string.IsNullOrEmpty(newClientID))
            {
                clientID = newClientID; 
                await connection.InvokeAsync("JoinChat", clientID);
            }
            Console.Write("Enter receiver ID (or press Enter to keep previous): ");
            var newReceiverID = Console.ReadLine();
            if (!string.IsNullOrEmpty(newReceiverID))
            {
                receiverID = newReceiverID; 
            }

            Console.Write("Enter message: ");
            var message = Console.ReadLine();
            if (string.IsNullOrEmpty(message)) break;

            await connection.InvokeAsync("SendMessage", clientID, receiverID, message);
        }

        await connection.StopAsync();
    }
}