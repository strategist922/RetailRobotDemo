using Microsoft.ProjectOxford.SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSTC.Robot.Interactions.RobotBehaviors;
namespace MSTC.Robot.Interactions.RobotBehaviors.Speech
{
    public class SpeechInteration : IRobotInteraction
    {
        MicrophoneRecognitionClient _speechClient = null;
        OnPartialOutputReceived _partialOutputReceived = null;
        OnIntentReceived _intentReceived = null;
        OnOutputReceived _outputReveived = null;
        OnError _onError = null;
        public SpeechInteration(string language, string speechApiPrimaryKey, string speechApiSecondaryKey,
                                string luisApiId, string luisSubscriptionId)
        {
            _speechClient = SpeechRecognitionServiceFactory.CreateMicrophoneClientWithIntent(
                                                                    language,
                                                                    speechApiPrimaryKey,
                                                                    speechApiSecondaryKey,
                                                                    luisApiId,
                                                                    luisSubscriptionId);
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
            // Event handlers for speech recognition results
            this._speechClient.OnMicrophoneStatus += this.OnMicrophoneStatus;
            this._speechClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;
            this._speechClient.OnResponseReceived += this.OnMicDictationResponseReceivedHandler;
            this._speechClient.OnConversationError += this.OnConversationErrorHandler;
        }
        private void OnMicDictationResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            if (_outputReveived != null)
            {
                if (e.PhraseResponse != null && e.PhraseResponse.Results.Length > 0)
                {
                    FinalOutputEvemt evt = new FinalOutputEvemt();
                    evt.SetEventData(e.PhraseResponse.Results[0].DisplayText);
                    evt.IsCompleted = true;
                    _outputReveived(evt);
                }
            }
            _speechClient.EndMicAndRecognition();
        }

        private void OnConversationErrorHandler(object sender, SpeechErrorEventArgs e)
        {
            if(_onError != null)
            {
                _onError(new ErrorEvent()
                {
                    ErrorMessage = $"{e.SpeechErrorCode} : {e.SpeechErrorText}",
                    InnerException = null
                });
            }
        }

        private void OnPartialResponseReceivedHandler(object sender, PartialSpeechResponseEventArgs e)
        {
            if(_partialOutputReceived != null)
            {
                PartialOutputEvent evt = new PartialOutputEvent();
                evt.SetEventData(e.PartialResult);
                evt.IsCompleted = false;    
                _partialOutputReceived(evt);
            }
        }

        private void OnMicrophoneStatus(object sender, MicrophoneEventArgs e)
        {
        }

        private void OnIntentHandler(object sender, SpeechIntentEventArgs e)
        {
            if(_intentReceived != null)
            {
                IntentEvent evt = new IntentEvent()
                {
                    Result = e.Payload
                };

                _intentReceived(evt);
            }
            _speechClient.EndMicAndRecognition();
        }



        public void StartInput(OnPartialOutputReceived OnPartialOutputReceivedHandler,
            OnOutputReceived OnFinalOutputReceivedHandler,
            OnIntentReceived OnIntentReceivedHandler,
            OnError OnErrorHandler)
        {
            _partialOutputReceived = OnPartialOutputReceivedHandler;
            _intentReceived = OnIntentReceivedHandler;
            _outputReveived = OnFinalOutputReceivedHandler;
            _onError = OnErrorHandler;

            _speechClient.StartMicAndRecognition();
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

        public void StartOutput(OutputData data)
        {
            //No vision output interaction is required
            
        }
    }
}
