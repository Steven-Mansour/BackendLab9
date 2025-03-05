// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.SignalR.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5250/chat")
            .Build();

        connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Console.WriteLine($"{user}: {message}");
        });

        await connection.StartAsync();
        Console.WriteLine("Connected to chat. Type your messages...");

        while (true)
        {
            var message = Console.ReadLine();
            if (string.IsNullOrEmpty(message)) break;

            await connection.InvokeAsync("SendMessage", "Client1", message);
        }

        await connection.StopAsync();
    }
}