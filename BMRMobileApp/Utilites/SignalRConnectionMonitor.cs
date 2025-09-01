using BMRMobileApp.InterFaces;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Utilites
{
    public class SignalRConnectionMonitor : IConnectionMonitorService
    {
        private readonly HubConnection _connection;
        public event Action<bool>? ConnectionStatusChanged;

        public bool IsConnected => _connection?.State == HubConnectionState.Connected;

        public SignalRConnectionMonitor()
        {
            _connection = new HubConnectionBuilder()
                 .WithUrl($"{ConfigurationManager.ChatHubModel.SingularAppUrl}{ConfigurationManager.ChatHubModel.SingularChatHub}", options =>
                 {
                     options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                 }) // Replace with your actual hub URL

                .WithAutomaticReconnect()
                .Build();

            _connection.Reconnecting += error =>
            {
                ConnectionStatusChanged?.Invoke(false);
                return Task.CompletedTask;
            };

            _connection.Reconnected += connectionId =>
            {
                ConnectionStatusChanged?.Invoke(true);
                return Task.CompletedTask;
            };

            _connection.Closed += error =>
            {
                ConnectionStatusChanged?.Invoke(false);
                return Task.CompletedTask;
            };
        }

        public async Task StartMonitoringAsync()
        {
            await _connection.StartAsync();
            ConnectionStatusChanged?.Invoke(IsConnected);
        }
    }

}
