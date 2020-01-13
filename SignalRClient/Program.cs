using Microsoft.AspNet.SignalR.Client;
using System;
using System.Net;
using System.Threading.Tasks;

public class MainClass
{
    static void Main(string[] args)
    {
        MainAsync();
        Console.ReadLine();
    }

    static void MainAsync()
    {
        try
        {
            var hubConnection = new HubConnection("http://192.168.0.102");
            IHubProxy notificationHubProxy = hubConnection.CreateHubProxy("notification");
            notificationHubProxy.On<string>("say", data => Console.WriteLine(data));
            Console.WriteLine("Connecting...");
            hubConnection.Start().Wait();
            Console.WriteLine("Say...");
            notificationHubProxy.Invoke("say", "Bonjour a vous 3").Wait();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}