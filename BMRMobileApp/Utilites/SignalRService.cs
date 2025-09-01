//using Microsoft.AspNetCore.SignalR;
//using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Utilites
{
    public class SignalRService
    {
        private HubConnection _connection;

        public async Task InitAsync()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($"{ConfigurationManager.ChatHubModel.SingularAppUrl}{ConfigurationManager.ChatHubModel.SingularChatHub}", options =>
                {
                    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                }) // Replace with your actual hub URL
                 
                .WithAutomaticReconnect()
                .Build();

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"Message from {user}: {message}");
            });

            try
            {
              // var startTask = _connection.StartAsync();
               // if (await Task.WhenAny(startTask, Task.Delay(1000)) != startTask)
             //  {
                 //   throw new TimeoutException("SignalR connection timed out.");
          //   }
                //await _connection.StartAsync();
                _connection.StartAsync().GetAwaiter().GetResult();
                Console.WriteLine("SignalR connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }
        }

        public async Task SendMessageAsync(string user, string message)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendMessage", user, message);
            }
        }

    }
}
