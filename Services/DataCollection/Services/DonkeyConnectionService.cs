using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Websocket.Client;


namespace DataManager.Services.DataCollection.Services
{
    public class DonkeyConnectionService
    {
        private WebsocketClient _client;
        public bool IsConnected { get; private set; }
        public event Action<string> OnRawMessage; // CameraService가 구독
        public async Task ConnectAsync()
        {
            _client = new WebsocketClient(new Uri("ws://localhost:8887/wsDrive"));
            _client.MessageReceived.Subscribe(msg =>
            {
                OnRawMessage?.Invoke(msg.Text);
            });
            await _client.Start();
            IsConnected = true;
        }
        public void SendControl(float angle, float throttle, bool recording)
        {
            if (!IsConnected) return;
            var cmd = new { angle, throttle, drive_mode = "user", recording };
            _client.Send(JsonConvert.SerializeObject(cmd));
        }
        public void Disconnect()
        {
            _client?.Dispose();
            IsConnected = false;
        }
    }
}