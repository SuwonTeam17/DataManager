using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DataManager.Services.DataCollection.Services
{
    public class CameraService
    {
        public event Action<Bitmap> OnFrameReceived;
        public event Action<byte[]> OnRawFrameReceived;

        private CancellationTokenSource _cts;
        private Task _streamTask;

        // static 제거 — 인스턴스마다 독립적인 HttpClient
        private HttpClient _http;

        public void ProcessMessage(string json) { }

        public void StartStream(string url = "http://localhost:8887/video")
        {
            StopStream(); // 이전 스트림 완전 종료 대기

            // HttpClient도 새로 생성 (이전 연결 상태 완전 초기화)
            _http = new HttpClient
            {
                Timeout = System.Threading.Timeout.InfiniteTimeSpan
            };

            _cts = new CancellationTokenSource();
            _streamTask = Task.Run(() => ReadMjpegStream(url, _cts.Token));
        }

        public void StopStream()
        {
            _cts?.Cancel();

            // 이전 스트림 Task가 완전히 끝날 때까지 대기 (최대 2초)
            try { _streamTask?.Wait(2000); } catch { }

            _cts = null;
            _streamTask = null;

            // HttpClient 정리
            _http?.Dispose();
            _http = null;
        }

        private async Task ReadMjpegStream(string url, CancellationToken token)
        {
            try
            {
                using var response = await _http.GetAsync(
                    url, HttpCompletionOption.ResponseHeadersRead, token);
                using var stream = await response.Content.ReadAsStreamAsync();
                var accumulator = new List<byte>(1024 * 512);
                var readBuffer = new byte[8192];

                while (!token.IsCancellationRequested)
                {
                    int bytesRead = await stream.ReadAsync(
                        readBuffer, 0, readBuffer.Length, token);
                    if (bytesRead == 0) break;
                    for (int i = 0; i < bytesRead; i++)
                        accumulator.Add(readBuffer[i]);
                    ExtractFrames(accumulator);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CAM] 오류: {ex.Message}");
            }
        }

        private void ExtractFrames(List<byte> buf)
        {
            while (true)
            {
                int start = -1;
                for (int i = 0; i < buf.Count - 1; i++)
                {
                    if (buf[i] == 0xFF && buf[i + 1] == 0xD8)
                    { start = i; break; }
                }
                if (start == -1) { buf.Clear(); break; }

                int end = -1;
                for (int i = start + 2; i < buf.Count - 1; i++)
                {
                    if (buf[i] == 0xFF && buf[i + 1] == 0xD9)
                    { end = i + 1; break; }
                }
                if (end == -1) break;

                int frameLen = end - start + 1;
                byte[] frameRaw = new byte[frameLen];
                buf.CopyTo(start, frameRaw, 0, frameLen);
                buf.RemoveRange(0, end + 1);

                OnRawFrameReceived?.Invoke(frameRaw);
                DecodeBitmap(frameRaw);
            }
        }

        private void DecodeBitmap(byte[] frameBytes)
        {
            try
            {
                Bitmap bmp;
                using (var ms = new MemoryStream(frameBytes))
                using (var tmp = new Bitmap(ms))
                    bmp = (Bitmap)tmp.Clone();
                OnFrameReceived?.Invoke(bmp);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CAM] 디코딩 오류: {ex.Message}");
            }
        }
    }
}