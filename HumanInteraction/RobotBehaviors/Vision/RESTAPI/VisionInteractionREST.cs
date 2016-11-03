using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MSTC.Robot.Interactions.RobotBehaviors.Vision
{
    public class VisionInteractionREST : IRobotInteraction
    {
        private string SubscriptionKey = null;
        private string PersonGroupId = null;
        public VisionInteractionREST(string subscriptionKey, string personGroupId)
        {
            SubscriptionKey = subscriptionKey;
            PersonGroupId = personGroupId;
        }
        private string TOKEN = null;
        HttpClient CreateHttpClient()
        {
            HttpClient c = new HttpClient(); ;
            c.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", SubscriptionKey);
            return c;
        }
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

        public void Dispose()
        {
            
        }


        public async Task StopInputAsync(OutputData data)
        {
            throw new NotImplementedException();
        }

        public void StopInput()
        {
            throw new NotImplementedException();
        }

        public async Task StartInputAsync()
        {
            throw new NotImplementedException();
        }
        private async Task<string> HandleResponseAsync(HttpResponseMessage resp)
        {
            var respText = await resp.Content.ReadAsStringAsync();
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (OnErrorHandler != null)
                {
                    OnErrorHandler(new ErrorEvent()
                    {
                        ErrorMessage = respText
                    });
                }
                return respText;
            }
            else
            {
                if (OnPartialOutputReceivedHandler != null)
                {
                    OnPartialOutputReceivedHandler(new PartialOutputEvent()
                    {
                        EventData = Encoding.UTF8.GetBytes(respText)
                    });
                }
                return respText;
            }
        }
        private async Task<string> DetectAsync(Stream stream)
        {
            var faceDetectURL = $"https://api.projectoxford.ai/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=true&returnFaceAttributes=age,gender,smile,facialHair,headPose,glasses";
            var httpClient = CreateHttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, faceDetectURL)
            {
                Content = new StreamContent(stream)
            };
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var resp = await httpClient.SendAsync(requestMessage);

            return await HandleResponseAsync(resp);
        }
        private async Task<string> IdentifyAsync(string[] faceIds)
        {
            var faceDetectURL = $"https://api.projectoxford.ai/face/v1.0/identify";
            var httpClient = CreateHttpClient();
            var request = new
                            {
                                personGroupId = PersonGroupId,
                                faceIds = faceIds,
                                maxNumOfCandidatesReturned = 1,
                                confidenceThreshold = 0.5
                            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, faceDetectURL)
            {
                Content = new StringContent(JsonConvert.SerializeObject(request))
            };
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var resp = await httpClient.SendAsync(requestMessage);
            return await HandleResponseAsync(resp);
        }
        private async Task<string> FindPersonAsync(string personId)
        {
            var findPersonURL = $"https://api.projectoxford.ai/face/v1.0/persongroups/{PersonGroupId}/persons/{personId}";
            var httpClient = CreateHttpClient();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, findPersonURL);
            //requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var resp = await httpClient.SendAsync(requestMessage);

            return await HandleResponseAsync(resp);
        }
        public async Task StartInputAsync(Stream stream)
        {
            var respText  = await DetectAsync(stream);
            dynamic faces = JsonConvert.DeserializeObject(respText);
            List<string> faceIds = new List<string>();
            foreach(var face in faces)
            {
                string id = (string)face.faceId;
                var identifyResult = await IdentifyAsync(new string[] { id });
                dynamic persons = JsonConvert.DeserializeObject(identifyResult);
                face.personFound = new Newtonsoft.Json.Linq.JArray();
                foreach (var person in persons)
                {
                    if (person.candidates.Count > 0)
                    {
                        //var candidate = candidates.FirstOrDefault();
                        foreach (var candidate in person.candidates)
                        {
                            string candidateId = candidate.personId;
                            var personFound = await FindPersonAsync(candidateId);
                            face.personFound.Add(JsonConvert.DeserializeObject(personFound));
                        }
                    }
                }
            }
            var result = JsonConvert.SerializeObject(faces);
            if(OnFinalOutputReceivedHandler != null)
            {
                OnFinalOutputReceivedHandler(new FinalOutputEvent
                {
                    EventData = Encoding.UTF8.GetBytes(result),
                    IsCompleted = true
                });
            }
        }
    }
}
