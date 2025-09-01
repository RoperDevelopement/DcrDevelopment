using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Http.Connections;
 namespace BMRMobileApp.Utilites
{
    public class ChatService
    {
        private HubConnection connection;

        public event Action<bool>? ConnectionStatusChanged;

        public async Task InitializeAsync()
        {
            connection = new HubConnectionBuilder()
                 .WithUrl($"{ConfigurationManager.ChatHubModel.SingularAppUrl}{ConfigurationManager.ChatHubModel.SingularChatHub}", options =>
                 {
                     options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                 }) // Replace with your actual hub URL

                .WithAutomaticReconnect()
                .Build();

            connection.Reconnecting += error =>
            {
                ConnectionStatusChanged?.Invoke(false);
                return Task.CompletedTask;
            };

            connection.Reconnected += connectionId =>
            {
                ConnectionStatusChanged?.Invoke(true);
                return Task.CompletedTask;
            };

           connection.Closed += error =>
            {
                ConnectionStatusChanged?.Invoke(false);
                return Task.CompletedTask;
            };

            await connection.StartAsync();
            ConnectionStatusChanged?.Invoke(true);
        }

        public bool IsConnected => connection?.State == HubConnectionState.Connected;
    }

}
