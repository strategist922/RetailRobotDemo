using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MSTC.Robot.Interactions.RobotBehaviors.Speech.RESTAPI
{
    public class SpeechInterationREST :IRobotInteraction
    {
        private string SUBSCRIPTION_KEY = null;
        private string TOKEN;
        private string Language = null;

        public OnPartialOutputReceived OnPartialOutputReceivedHandler
        {
            get;set;
        }

        public OnOutputReceived OnFinalOutputReceivedHandler
        {
            get; set;
        }

        public OnIntentReceived OnIntentReceivedHandler
        {
            get; set;
        }

        public OnError OnErrorHandler
        {
            get; set;
        }

        public SpeechInterationREST(string key1, string language)
        {
            SUBSCRIPTION_KEY = key1;
            Language = language;
        }
        private async Task<string> AcquireTokenAsync()
        {
            var url = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";//"https://api.projectoxford.ai/speech/v0/internalIssueToken";// 
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", "883f2099004446d798ebdce23ea39a07");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Length", "0");
            var resp = await httpClient.PostAsync(url, null);
                //new StringContent(
                //                                            "",Encoding.UTF8, "application/x-www-form-urlencoded"));
            var text = resp.Content as StreamContent;
            var body = await text.ReadAsStringAsync();
            return body;
            //dynamic o = JsonConvert.DeserializeObject(body);
            //return o.access_token;
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
        
        public async Task StartInputAsync(Stream stream)
        {
            //https://speech.platform.bing.com/recognize
            if (string.IsNullOrEmpty(TOKEN))
            {
                TOKEN = await AcquireTokenAsync();
            }
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {TOKEN}");
            //Content-Type: audio/wav; samplerate=16000
            var resp = await httpClient.PostAsync($"https://speech.platform.bing.com/recognize?version=3.0&instanceid={Guid.NewGuid().ToString()}&scenarios=websearch&appid=D4D52672-91D7-4C74-8AD8-42B1D98141A5&format=json&device.os=win10&requestid={Guid.NewGuid().ToString()}&locale=en-US",
                                    new StreamContent(stream));
            using (var sr = new StreamReader(await resp.Content.ReadAsStreamAsync())) {
                var text = sr.ReadToEnd();
                if (OnPartialOutputReceivedHandler != null)
                {
                    OnPartialOutputReceivedHandler(new PartialOutputEvent()
                    {
                        EventData = Encoding.UTF8.GetBytes(text),
                        IsCompleted = true
                    });
                }
            }
        }
        
        public Task StopInputAsync(OutputData data)
        {
            throw new NotImplementedException();
        }

        public void StopInput()
        {
            throw new NotImplementedException();
        }

        public Task StartInputAsync()
        {
            throw new NotImplementedException();
        }

    }
}
