using myoshidan.IBM.Watson.STT.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace myoshidan.IBM.Watson.STT.Models
{
    public class IBMWatsonSpeechToTextWebsocketService
    {
        public string Region { get; set; }
        public string AccessToken { get; set; }
        public string Model { get; set; }
        public bool IsConnected { get; set; }
        public string Transcipt { get; private set; }
        public int ResultIndex { get; set; }


        private static string BaseUrlWithModel = "wss://{0}/speech-to-text/api/v1/recognize?access_token={1}&model={2}";
        private static string BaseUrl = "wss://{0}/speech-to-text/api/v1/recognize?access_token={1}";
        private static readonly ArraySegment<byte> OpenMessage = new ArraySegment<byte>(Encoding.UTF8.GetBytes("{\"action\": \"start\", \"content-type\": \"audio/wav\", \"interim_results\": true}"));
        private static readonly ArraySegment<byte> CloseMessage = new ArraySegment<byte>(Encoding.UTF8.GetBytes("{\"action\": \"stop\"}"));

        private static ClientWebSocket ws = new ClientWebSocket();

        public IBMWatsonSpeechToTextWebsocketService(string region, string token,string model)
        {
            this.Region = region;
            this.AccessToken = token;
            this.Model = model;

            ws = new ClientWebSocket();
        }


        public async Task StartConnection()
        {

            Uri url;
            if (string.IsNullOrEmpty(Model))
            {
                url = new Uri(string.Format(BaseUrl, Region, AccessToken));
            }
            else
            {
                url = new Uri(string.Format(BaseUrlWithModel, Region, AccessToken, Model));
            }

            await ws.ConnectAsync(url, CancellationToken.None);
            //Console.WriteLine(ws.State);

            await ws.SendAsync(OpenMessage, WebSocketMessageType.Text, true, CancellationToken.None);
            await HandleCallback();
            return;
        }

        public async Task CloseConnection()
        {
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
        }

        public async Task StartAudioFileSend(string filePath)
        {
            await Task.WhenAll(SendAudioToWatson(filePath), HandleCallback());
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
        }

        public async Task SendAudioToWatson(string filePath)
        {
            var count = 1;
            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var bytes = new byte[1048576];
                while (fileStream.Read(bytes, 0, bytes.Length) > 0)
                {
                    await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true, CancellationToken.None);
                    count++;
                }
                await ws.SendAsync(CloseMessage, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task SendAudioToWatson(byte[] bytes)
        {
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true, CancellationToken.None);
            return;
        }

        public async Task HandleCallback()
        {
            var buffer = new byte[1048576];
            while (true)
            {
                var segment = new ArraySegment<byte>(buffer);
                WebSocketReceiveResult result;
                try
                {
                    result = await ws.ReceiveAsync(segment, CancellationToken.None);
                    //Console.WriteLine($"Recieve MessageType:{result.MessageType},BufLen:{result.Count}");
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (result.MessageType == WebSocketMessageType.Close) return;

                var count = result.Count;
                while (!result.EndOfMessage)
                {
                    if (count >= buffer.Length)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "count >= buffer.Length!!!!!", CancellationToken.None);
                        return;
                    }

                    segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                    result = await ws.ReceiveAsync(segment, CancellationToken.None);
                    count += result.Count;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, count);
                if (IsDelimeter(message))
                {
                    IsConnected = true;
                    return;
                }
                var jsonObj = JsonConvert.DeserializeObject<StreamingRecognizeResponse>(message);
                Transcipt = jsonObj.results.FirstOrDefault().alternatives.FirstOrDefault().transcript;
                ResultIndex = jsonObj.result_index;
            }
        }

        private static bool IsDelimeter(string json) => JsonConvert.DeserializeObject<dynamic>(json).state == "listening";
    }
}
