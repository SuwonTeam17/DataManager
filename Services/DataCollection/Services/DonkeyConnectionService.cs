using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Websocket.Client;

namespace DataManager.Services.DataCollection.Services
{
    public class DonkeyConnectionService
    {
        private WebsocketClient _client;
        private IDisposable _messageSubscription; // 구독 핸들 명시적 관리

        public bool IsConnected { get; private set; }
        public event Action<string> OnRawMessage;

        public async Task ConnectAsync()
        {
            // 이전 연결 완전 정리 후 새로 시작
            Disconnect();

            _client = new WebsocketClient(new Uri("ws://localhost:8887/wsDrive"));

            // 재연결 비활성화 (수동으로만 연결/해제 제어)
            _client.ReconnectTimeout = null;
            _client.IsReconnectionEnabled = false;

            // 구독 핸들을 저장해서 나중에 명시적으로 해제
            _messageSubscription = _client.MessageReceived.Subscribe(msg =>
            {
                OnRawMessage?.Invoke(msg.Text);
            });

            await _client.Start();
            IsConnected = true;
        }

        public void SendControl(float angle, float throttle, bool recording)
        {
            if (!IsConnected || _client == null) return;
            var cmd = new { angle, throttle, drive_mode = "user", recording };
            _client.Send(JsonConvert.SerializeObject(cmd));
        }

        public void Disconnect()
        {
            IsConnected = false;

            // 구독 먼저 해제
            _messageSubscription?.Dispose();
            _messageSubscription = null;

            // 클라이언트 정리
            try { _client?.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "disconnect"); } catch { }
            _client?.Dispose();
            _client = null;
        }
    }
}