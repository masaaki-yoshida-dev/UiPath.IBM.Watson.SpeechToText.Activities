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
    /// <summary>
    /// IBM Watson Speech To Text Websocket Service
    /// </summary>
    public class IBMWatsonSpeechToTextWebsocketService
    {
        #region Properties
        public string Region { get; set; }
        public string AccessToken { get; set; }
        public string Model { get; set; }
        public string Transcipt { get; private set; }
        public int ResultIndex { get; set; }
        #endregion

        #region private fields
        private static string BaseUrlWithModel = "wss://{0}/speech-to-text/api/v1/recognize?access_token={1}&model={2}";
        private static string BaseUrl = "wss://{0}/speech-to-text/api/v1/recognize?access_token={1}";
        private static readonly ArraySegment<byte> OpenMessage = new ArraySegment<byte>(Encoding.UTF8.GetBytes("{\"action\": \"start\", \"content-type\": \"audio/wav\",\"inactivity_timeout\": -1, \"interim_results\": true}"));
        private static readonly ArraySegment<byte> CloseMessage = new ArraySegment<byte>(Encoding.UTF8.GetBytes("{\"action\": \"stop\"}"));
        private static ClientWebSocket ws = new ClientWebSocket();
        #endregion

        #region Constructor

        /// <summary>
        /// IBMWatsonSpeechToTextWebsocketService Constructor
        /// </summary>
        /// <param name="region"></param>
        /// <param name="token"></param>
        /// <param name="model"></param>
        public IBMWatsonSpeechToTextWebsocketService(string region, string token,string model)
        {
            this.Region = region;
            this.AccessToken = token;
            this.Model = model;

            ws = new ClientWebSocket();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Start Websocket connection and send start message.
        /// </summary>
        /// <returns></returns>
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
            await ws.SendAsync(OpenMessage, WebSocketMessageType.Text, true, CancellationToken.None);
            await HandleCallback();
            return;
        }

        /// <summary>
        /// send websocket connection close message.
        /// </summary>
        /// <returns></returns>
        public async Task CloseConnection()
        {
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
        }

        /// <summary>
        /// Open and Send audiofile to Speech to text websocket and recieve Transcript(and set property)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task StartAudioFileSend(string filePath)
        {
            await Task.WhenAll(SendAudioFileToWatson(filePath), HandleCallback());
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
        }

        /// <summary>
        /// send audio stream to Speech to text websocket and recieve Transctiot (and set property)
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public async Task SendAudioToWatson(byte[] bytes)
        {
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true, CancellationToken.None);
            return;
        }

        /// <summary>
        /// recieve websocket message until by websocket is closed.
        /// </summary>
        /// <returns></returns>
        public async Task HandleCallback()
        {
            var buffer = new byte[1048576];
            while (true)
            {
                var segment = new ArraySegment<byte>(buffer);
                var result = await ws.ReceiveAsync(segment, CancellationToken.None);

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

                if (IsDelimeter(message))return;
                
                var jsonObj = JsonConvert.DeserializeObject<StreamingRecognizeResponse>(message);
                Transcipt = jsonObj.results.FirstOrDefault().alternatives.FirstOrDefault().transcript;
                ResultIndex = jsonObj.result_index;
            }
        }
        #endregion

        #region private Methods
        /// <summary>
        /// Open AudioFile and Send audio stream to Speech to text service 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task SendAudioFileToWatson(string filePath)
        {
            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var bytes = new byte[1048576];
                while (fileStream.Read(bytes, 0, bytes.Length) > 0)
                {
                    await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true, CancellationToken.None);
                }
                await ws.SendAsync(CloseMessage, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        /// <summary>
        /// Check json response is 'listening' or not.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static bool IsDelimeter(string json) => JsonConvert.DeserializeObject<dynamic>(json).state == "listening";

        #endregion

    }
}
