using BMRMobileApp.InterFaces;
using BMRMobileApp.Models;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Utilites
{
    public class SignalRChatRelay : IChatRelayService
    {
        private HubConnection _connection;
        public event Action<ChatMessageModel>? MessageReceived;
        public event Action<bool>? ConnectionStatusChanged;

        public bool IsConnected => _connection?.State == HubConnectionState.Connected;

        public async Task StartAsync()
        {
            _connection = new HubConnectionBuilder()
                   .WithUrl($"{ConfigurationManager.ChatHubModel.SingularAppUrl}{ConfigurationManager.ChatHubModel.SingularChatHub}", options =>
                   {
                       options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                   }) 
                .WithAutomaticReconnect()
                .Build();

            _connection.On<string, string>("ReceiveMessage", (user, text) =>
            {
                var message = new ChatMessageModel { Sender = user, SenderMessage = text ,TimesMessageSent=DateTime.Now.ToString()};
                MessageReceived?.Invoke(message);
            });

            _connection.Reconnecting += _ => { ConnectionStatusChanged?.Invoke(false); return Task.CompletedTask; };
            _connection.Reconnected += _ => { ConnectionStatusChanged?.Invoke(true); return Task.CompletedTask; };
            _connection.Closed += _ => { ConnectionStatusChanged?.Invoke(false); return Task.CompletedTask; };

            await _connection.StartAsync();
            ConnectionStatusChanged?.Invoke(true);
        }

        public async Task SendMessageAsync(ChatMessageModel message)
        {
            if (IsConnected)
                await _connection.InvokeAsync("SendMessage", message.Sender, message.SenderMessage);
        }
    }

}
