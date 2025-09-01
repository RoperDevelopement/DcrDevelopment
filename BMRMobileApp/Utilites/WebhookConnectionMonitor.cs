using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
namespace BMRMobileApp.Utilites
{
    public class WebhookConnectionMonitor : IConnectionMonitorService
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _pingUrl;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);
        private CancellationTokenSource _cts = new();

        public event Action<bool>? ConnectionStatusChanged;
        public bool IsConnected { get; private set; }

        public WebhookConnectionMonitor(string pingUrl)
        {
            _pingUrl = pingUrl;
        }

        public async Task StartMonitoringAsync()
        {
            _cts = new CancellationTokenSource();

            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var response = await _httpClient.GetAsync(_pingUrl, _cts.Token);
                    bool newStatus = response.IsSuccessStatusCode;

                    if (newStatus != IsConnected)
                    {
                        IsConnected = newStatus;
                        ConnectionStatusChanged?.Invoke(IsConnected);
                    }
                }
                catch
                {
                    if (IsConnected)
                    {
                        IsConnected = false;
                        ConnectionStatusChanged?.Invoke(false);
                    }
                }

                await Task.Delay(_interval, _cts.Token);
            }
        }

        public void StopMonitoring() => _cts.Cancel();
    }

}
