using Microsoft.ProjectOxford.SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSTC.Robot.Interactions.RobotBehaviors;
using System.IO;

namespace MSTC.Robot.Interactions.RobotBehaviors.Speech
{
    public partial class SpeechInteration : IRobotInteraction
    {
        MicrophoneRecognitionClient _speechClient = null;
        private string SPEECHAPI_KEY1 = null;
        private string SPEECHAPI_KEY2 = null;
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

        public SpeechInteration(string language, string speechApiPrimaryKey, string speechApiSecondaryKey,
                                string luisApiId, string luisSubscriptionId)
        {
            _speechClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(SpeechRecognitionMode.LongDictation,
                                                                                    language,
                                                                                    speechApiPrimaryKey,
                                                                                    speechApiSecondaryKey);
                //.CreateMicrophoneClientWithIntent(
                //                                                    language,
                //                                                    speechApiPrimaryKey,
                //                                                    speechApiSecondaryKey,
                //                                                    luisApiId,
                //                                                    luisSubscriptionId);
            SPEECHAPI_KEY1 = speechApiPrimaryKey;
            SPEECHAPI_KEY2 = speechApiSecondaryKey;
            Language = language;
            this._speechClient.OnIntent += this.OnIntentHandler;
            
            // Event handlers for speech recognition results
            this._speechClient.OnMicrophoneStatus += this.OnMicrophoneStatus;
            this._speechClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;
            this._speechClient.OnResponseReceived += this.OnMicDictationResponseReceivedHandler;
            this._speechClient.OnConversationError += this.OnConversationErrorHandler;
        }
        public SpeechInteration(string language, string speechApiPrimaryKey, string speechApiSecondaryKey)
        {
            _speechClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(SpeechRecognitionMode.LongDictation,
                                                                    language,
                                                                    speechApiPrimaryKey,
                                                                    speechApiSecondaryKey);
            SPEECHAPI_KEY1 = speechApiPrimaryKey;
            SPEECHAPI_KEY2 = speechApiSecondaryKey;
            Language = language;
            // Event handlers for speech recognition results
            this._speechClient.OnMicrophoneStatus += this.OnMicrophoneStatus;
            this._speechClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;
            this._speechClient.OnResponseReceived += this.OnMicDictationResponseReceivedHandler;
            this._speechClient.OnConversationError += this.OnConversationErrorHandler;
        }
        private void OnMicDictationResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            if (OnFinalOutputReceivedHandler != null)
            {
                if (e.PhraseResponse != null && e.PhraseResponse.Results.Length > 0)
                {
                    FinalOutputEvent evt = new FinalOutputEvent();
                    evt.SetEventData(e.PhraseResponse.Results[0].DisplayText);
                    evt.IsCompleted = true;
                    OnFinalOutputReceivedHandler(evt);
                }
            }
            _speechClient.EndMicAndRecognition();
        }

        private void OnConversationErrorHandler(object sender, SpeechErrorEventArgs e)
        {
            if(OnErrorHandler != null)
            {
                OnErrorHandler(new ErrorEvent()
                {
                    ErrorMessage = $"{e.SpeechErrorCode} : {e.SpeechErrorText}",
                    InnerException = null
                });
            }
        }

        private void OnPartialResponseReceivedHandler(object sender, PartialSpeechResponseEventArgs e)
        {
            if(OnPartialOutputReceivedHandler != null)
            {
                PartialOutputEvent evt = new PartialOutputEvent();
                evt.SetEventData(e.PartialResult);
                evt.IsCompleted = false;
                OnPartialOutputReceivedHandler(evt);
            }
        }

        private void OnMicrophoneStatus(object sender, MicrophoneEventArgs e)
        {
        }

        private void OnIntentHandler(object sender, SpeechIntentEventArgs e)
        {
            if(OnIntentReceivedHandler != null)
            {
                IntentEvent evt = new IntentEvent()
                {
                    Result = e.Payload
                };

                OnIntentReceivedHandler(evt);
            }
            _speechClient.EndMicAndRecognition();
        }
        
        public void StopInput()
        {
            _speechClient.EndMicAndRecognition();
        }
        public void Dispose()
        {
            _speechClient.EndMicAndRecognition();
            _speechClient.Dispose();
        }

        public async Task StartInputAsync()
        {
            _speechClient.StartMicAndRecognition();
        }

        public Task StartInputAsync(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
