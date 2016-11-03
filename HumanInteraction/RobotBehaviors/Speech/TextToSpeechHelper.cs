using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MSTC.Robot.Interactions.RobotBehaviors.Speech
{
    public partial class TextToSpeechHelper
    {
        private string BaseUri = "https://speech.platform.bing.com/synthesize";
        private string KEY1 = null;
        private string KEY2 = null;
        private string TOKEN = null;
        private string Language = null;

        public TextToSpeechHelper(string key1, string key2, string language)
        {
            KEY1 = key1;
            KEY2 = key2;
            Language = language;
        }

        public async Task PostAsync(string text)
        {
            var speakerName = speakerNames[Language]["Female"];
            //var baseXml = $"<speak version='1.0' xml:lang='en-us'><voice xml:lang='{Language}' xml:gender='Female' name='Microsoft Server Speech Text to Speech Voice ({Language}, Yating, Apollo)'>{text}</voice></speak>";
            var baseXml = $"<speak version='1.0' xml:lang='en-us'><voice xml:lang='{Language}' xml:gender='Female' name='{speakerName}'>{text}</voice></speak>";
            if (String.IsNullOrEmpty(TOKEN))
            {
                TOKEN = await AcquireTokenAsync();
            }
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Microsoft-OutputFormat", "riff-16khz-16bit-mono-pcm");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/ssml+xml");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {TOKEN}");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "RetailDemo");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, BaseUri)
            {
                Content = new StringContent(baseXml)
            };
            var resp = client.SendAsync(requestMessage);
            await resp.ContinueWith(
                async (respMessage, token) =>
                {
                    if (respMessage.IsCompleted && respMessage.Result != null && respMessage.Result.IsSuccessStatusCode)
                    {
                        using (var httpStream = await respMessage.Result.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            var player = new SoundPlayer(httpStream);
                            player.PlaySync();//.Play();
                        }
                    }
                },
                System.Threading.CancellationToken.None
                );
        }
        

        private async Task<string> AcquireTokenAsync()
        {
            var url = "https://oxford-speech.cloudapp.net/token/issueToken";
            HttpClient httpClient = new HttpClient();

            var resp = await httpClient.PostAsync(url,
                    new StringContent(
                            $"grant_type=client_credentials&client_id={KEY1}&client_secret={KEY2}&scope=https%3A%2F%2Fspeech.platform.bing.com",
                            Encoding.UTF8,
                            "application/x-www-form-urlencoded"));
            var text = resp.Content as StreamContent;
            var body = await text.ReadAsStringAsync();

            dynamic o = JsonConvert.DeserializeObject(body);
            return o.access_token;
        }
    }
}
